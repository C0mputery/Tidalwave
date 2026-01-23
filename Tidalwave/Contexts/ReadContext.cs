using System;
using System.Runtime.CompilerServices;

namespace Tidalwave {
    public ref partial struct ReadContext {
        private readonly ReadOnlySpan<ulong> buffer;
        private int bitPosition;
        private int bitCapacity;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadContext(ReadOnlySpan<ulong> buffer) {
            this.buffer = buffer;
            bitPosition = 0;
            bitCapacity = buffer.Length * 64;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetBitCapacity(int bits) { bitCapacity = bits; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetBitPosition(int position) { bitPosition = position; }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetBitsRemaining() { return bitCapacity - bitPosition; }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool PeakBit() {
            int ulongIndex = bitPosition / TypeBitSizes.ULongSize;
            int bitOffset = bitPosition % TypeBitSizes.ULongSize;
            return (buffer[ulongIndex] & (1UL << bitOffset)) != 0;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ReadBit() {
            bool value = PeakBit();
            bitPosition++;
            return value;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong PeakBits(int count) {
            int ulongIndex = bitPosition / TypeBitSizes.ULongSize;
            int bitOffset = bitPosition % TypeBitSizes.ULongSize;

            ulong valueMask = count == TypeBitSizes.ULongSize ? ulong.MaxValue : (1UL << count) - 1;
            ulong result = (buffer[ulongIndex] >> bitOffset);

            int bitsAvailableInCurrent = TypeBitSizes.ULongSize - bitOffset;
            if (count > bitsAvailableInCurrent) { result |= (buffer[ulongIndex + 1] << bitsAvailableInCurrent); }

            return result & valueMask;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong ReadBits(int count) {
            ulong value = PeakBits(count);
            bitPosition += count;
            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool CheckForSpace(int bitsNeeded) { return GetBitsRemaining() >= bitsNeeded; }
    }
}