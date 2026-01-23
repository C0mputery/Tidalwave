using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Tidalwave {
    public ref partial struct WriteContext {
        private readonly Span<ulong> buffer;
        public int BitPosition { get; private set; }
        private readonly int bitCapacity;
        public WriteContext(Span<ulong> buffer) {
            this.buffer = buffer;
            BitPosition = 0;
            bitCapacity = buffer.Length * 64;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ThrowIfNoSpace(int bitsNeeded, string forWhat) {
            int bitsRemaining = bitCapacity - BitPosition;
            if (bitsRemaining < bitsNeeded) { throw new InsufficientWriteSpaceException(forWhat, bitsNeeded, bitsRemaining); }
        }
        
        /// <summary>
        /// Writes a single bit to the buffer.
        /// Assumes there is enough space in the buffer, caller must ensure this.
        /// </summary>
        /// <param name="bit"> The bit to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteBit(bool bit) {
            int ulongIndex = BitPosition / TypeBitSizes.ULongSize;
            int currentBitInUlong = BitPosition % TypeBitSizes.ULongSize;
            ulong mask = 1UL << currentBitInUlong; // only 1 at the bit position we want to write, zeros elsewhere
            ulong bitValue = bit ? 1UL : 0UL; // convert bool to ulong (0 or 1)
            buffer[ulongIndex] = (buffer[ulongIndex] & ~mask) // sets the bit at bitInUlong to 0
                                 | // if 1 on any side is 1, result is 1, else 0
                                 (bitValue << currentBitInUlong); // set the bit at bitInUlong to bitValue

            BitPosition++;
        }
        
        /// <summary>
        /// Writes the given number of bits from the value to the buffer.
        /// Assumes there is enough space in the buffer, caller must ensure this.
        /// Assumes count is between 1 and 64, inclusive, caller must ensure this.
        /// </summary>
        /// <param name="value"> The value containing the bits to write.</param>
        /// <param name="count"> The number of bits to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteBits(ulong value, int count) {
            ulong valueMask = count == TypeBitSizes.ULongSize ? ulong.MaxValue : (1UL << count) - 1;
            value &= valueMask;

            int ulongIndex = BitPosition / TypeBitSizes.ULongSize;
            int bitOffset = BitPosition % TypeBitSizes.ULongSize;
            
            ulong mask = valueMask << bitOffset;
            buffer[ulongIndex] = (buffer[ulongIndex] & ~mask) 
                                 |
                                 (value << bitOffset);

            int bitsUsedInCurrent = TypeBitSizes.ULongSize - bitOffset;
            if (count > bitsUsedInCurrent) {
                int bitsRemaining = count - bitsUsedInCurrent;
                ulong nextMask = (1UL << bitsRemaining) - 1;
                buffer[ulongIndex + 1] = (buffer[ulongIndex + 1] & ~nextMask) 
                                         |
                                         (value >> bitsUsedInCurrent);
            }

            BitPosition += count;
        }

        /// <summary>
        /// Reserves the given number of bits in the buffer by advancing the bit position.
        /// Does not write any data meaning if using a pooled buffer, the reserved bits may contain old data.
        /// Assumes there is enough space in the buffer, caller must ensure this.
        /// </summary>
        /// <param name="count"> The number of bits to reserve.</param>
        public void ReserveBits(int count) { BitPosition += count; }
        
        /// <summary>
        /// Sets the current bit position in the buffer.
        /// Assumes the given position is valid, caller must ensure this.
        /// </summary>
        /// <param name="bitPosition"> The bit position to set.</param>
        public void SetPosition(int bitPosition) { BitPosition = bitPosition; }

        /// <summary>
        /// Gets a span of bytes representing the written data in the buffer.
        /// This will include garbage bits in the last byte if the total number of bits written is not a multiple of 8.
        /// </summary>
        /// <returns></returns>
        public Span<byte> ToByte() {
            int relevantUlongs = (BitPosition + 63) / 64;
            Span<ulong> relevantBuffer = buffer.Slice(0, relevantUlongs);
            int totalBytes = (BitPosition + 7) / 8;
            return MemoryMarshal.Cast<ulong, byte>(relevantBuffer).Slice(0, totalBytes);
        }
    }

    public class InsufficientWriteSpaceException : Exception {
        public InsufficientWriteSpaceException(string failedThing, int requiredBits, int availableBits) : base($"Insufficient space to write {failedThing}. Required bits: {requiredBits}, Available bits: {availableBits}.") { }
    }
}