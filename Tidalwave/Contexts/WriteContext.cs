using System;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Tidalwave {
    public ref partial struct WriteContext {
        private readonly Span<ulong> buffer;
        private int bitPosition;
        private readonly int bitCapacity;
        public WriteContext(Span<ulong> buffer) {
            this.buffer = buffer;
            bitPosition = 0;
            bitCapacity = buffer.Length * 64;
        }
        
        private int BitsRemaining => bitCapacity - bitPosition;

        /// <summary>
        /// Writes a single bit to the buffer.
        /// Assumes there is enough space in the buffer, caller must ensure this.
        /// </summary>
        /// <param name="bit"> The bit to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteBit(bool bit) {
            int ulongIndex = bitPosition / TypeBitSizes.ULongSize;
            int currentBitInUlong = bitPosition % TypeBitSizes.ULongSize;
            ulong mask = 1UL << currentBitInUlong; // only 1 at the bit position we want to write, zeros elsewhere
            ulong bitValue = bit ? 1UL : 0UL; // convert bool to ulong (0 or 1)
            buffer[ulongIndex] = (buffer[ulongIndex] & ~mask) // sets the bit at bitInUlong to 0
                                 | // if 1 on any side is 1, result is 1, else 0
                                 (bitValue << currentBitInUlong); // set the bit at bitInUlong to bitValue

            bitPosition++;
        }
        
        /// <summary>
        /// Writes the given number of bits from the value to the buffer.
        /// Assumes there is enough space in the buffer, caller must ensure this.
        /// Assumes count is between 1 and 64, inclusive, caller must ensure this.
        /// </summary>
        /// <param name="value"> The value containing the bits to write.</param>
        /// <param name="count"> The number of bits to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteBits(ulong value, int count) {
            ulong valueMask = count == TypeBitSizes.ULongSize ? ulong.MaxValue : (1UL << count) - 1;
            value &= valueMask;

            int ulongIndex = bitPosition / TypeBitSizes.ULongSize;
            int bitOffset = bitPosition % TypeBitSizes.ULongSize;
            
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

            bitPosition += count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteBool(bool value) { WriteBit(value); }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteBools(ReadOnlySpan<bool> values) {
            const int numberOfValuesInUlong = TypeBitSizes.ULongSize / TypeBitSizes.BoolSize;
            int count = values.Length;
            int processed = 0;

            while (processed + numberOfValuesInUlong <= count) {
                ulong packed = 0;
                for (int i = 0; i < numberOfValuesInUlong; i++) {
                    if (values[processed + i]) {
                        packed |= (1UL << i);
                    }
                }
                WriteBits(packed, TypeBitSizes.ULongSize);
                processed += numberOfValuesInUlong;
            }

            int remaining = count - processed;
            if (remaining > 0) {
                ulong packed = 0;
                for (int i = 0; i < remaining; i++) {
                    if (values[processed + i]) {
                        packed |= (1UL << i);
                    }
                }
                WriteBits(packed, remaining * TypeBitSizes.BoolSize);
            }
        }
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteByte(byte value) { WriteBits(value, TypeBitSizes.ByteSize); }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteBytes(ReadOnlySpan<byte> values) {
            const int numberOfValuesInUlong = TypeBitSizes.ULongSize / TypeBitSizes.ByteSize;
            int count = values.Length;
            int processed = 0;

            while (processed + numberOfValuesInUlong <= count) {
                ulong packed = 0;
                for (int i = 0; i < numberOfValuesInUlong; i++) {
                    packed |= (ulong)values[processed + i] << (i * TypeBitSizes.ByteSize);
                }
                WriteBits(packed, TypeBitSizes.ULongSize);
                processed += numberOfValuesInUlong;
            }

            int remaining = count - processed;
            if (remaining > 0) {
                ulong packed = 0;
                for (int i = 0; i < remaining; i++) {
                    packed |= (ulong)values[processed + i] << (i * TypeBitSizes.ByteSize);
                }
                WriteBits(packed, remaining * TypeBitSizes.ByteSize);
            }
        }
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteSByte(sbyte value) { WriteBits((byte)value, TypeBitSizes.SByteSize); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteSBytes(ReadOnlySpan<sbyte> values) {
            const int numberOfValuesInUlong = TypeBitSizes.ULongSize / TypeBitSizes.SByteSize;
            int count = values.Length;
            int processed = 0;

            while (processed + numberOfValuesInUlong <= count) {
                ulong packed = 0;
                for (int i = 0; i < numberOfValuesInUlong; i++) {
                    packed |= (ulong)(byte)values[processed + i] << (i * TypeBitSizes.SByteSize);
                }
                WriteBits(packed, TypeBitSizes.ULongSize);
                processed += numberOfValuesInUlong;
            }

            int remaining = count - processed;
            if (remaining > 0) {
                ulong packed = 0;
                for (int i = 0; i < remaining; i++) {
                    packed |= (ulong)(byte)values[processed + i] << (i * TypeBitSizes.SByteSize);
                }
                WriteBits(packed, remaining * TypeBitSizes.SByteSize);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteShort(short value) { WriteBits((ushort)value, TypeBitSizes.ShortSize); }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteShorts(ReadOnlySpan<short> values) {
            const int numberOfValuesInUlong = TypeBitSizes.ULongSize / TypeBitSizes.ShortSize;
            int count = values.Length;
            int processed = 0;

            while (processed + numberOfValuesInUlong <= count) {
                ulong packed = 0;
                for (int i = 0; i < numberOfValuesInUlong; i++) {
                    packed |= (ulong)(ushort)values[processed + i] << (i * TypeBitSizes.ShortSize);
                }
                WriteBits(packed, TypeBitSizes.ULongSize);
                processed += numberOfValuesInUlong;
            }

            int remaining = count - processed;
            if (remaining > 0) {
                ulong packed = 0;
                for (int i = 0; i < remaining; i++) {
                    packed |= (ulong)(ushort)values[processed + i] << (i * TypeBitSizes.ShortSize);
                }
                WriteBits(packed, remaining * TypeBitSizes.ShortSize);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteUShort(ushort value) { WriteBits(value, TypeBitSizes.UShortSize); }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteUShorts(ReadOnlySpan<ushort> values) {
            const int numberOfValuesInUlong = TypeBitSizes.ULongSize / TypeBitSizes.UShortSize;
            int count = values.Length;
            int processed = 0;

            while (processed + numberOfValuesInUlong <= count) {
                ulong packed = 0;
                for (int i = 0; i < numberOfValuesInUlong; i++) {
                    packed |= (ulong)values[processed + i] << (i * TypeBitSizes.UShortSize);
                }
                WriteBits(packed, TypeBitSizes.UShortSize);
                processed += numberOfValuesInUlong;
            }

            int remaining = count - processed;
            if (remaining > 0) {
                ulong packed = 0;
                for (int i = 0; i < remaining; i++) {
                    packed |= (ulong)values[processed + i] << (i * TypeBitSizes.UShortSize);
                }
                WriteBits(packed, remaining * TypeBitSizes.UShortSize);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteInt(int value) { WriteBits((uint)value, TypeBitSizes.IntSize); }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteInts(ReadOnlySpan<int> values) {
            const int numberOfValuesInUlong = TypeBitSizes.ULongSize / TypeBitSizes.IntSize;
            int count = values.Length;
            int processed = 0;

            while (processed + numberOfValuesInUlong <= count) {
                ulong packed = 0;
                for (int i = 0; i < numberOfValuesInUlong; i++) {
                    packed |= (ulong)(uint)values[processed + i] << (i * TypeBitSizes.IntSize);
                }
                WriteBits(packed, TypeBitSizes.ULongSize);
                processed += numberOfValuesInUlong;
            }

            int remaining = count - processed;
            if (remaining > 0) {
                ulong packed = 0;
                for (int i = 0; i < remaining; i++) {
                    packed |= (ulong)(uint)values[processed + i] << (i * TypeBitSizes.IntSize);
                }
                WriteBits(packed, remaining * TypeBitSizes.IntSize);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteUInt(uint value) { WriteBits(value, TypeBitSizes.UIntSize); }
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteUInts(ReadOnlySpan<uint> values) {
            const int numberOfValuesInUlong = TypeBitSizes.ULongSize / TypeBitSizes.UIntSize;
            int count = values.Length;
            int processed = 0;

            while (processed + numberOfValuesInUlong <= count) {
                ulong packed = 0;
                for (int i = 0; i < numberOfValuesInUlong; i++) {
                    packed |= (ulong)values[processed + i] << (i * TypeBitSizes.UIntSize);
                }
                WriteBits(packed, TypeBitSizes.ULongSize);
                processed += numberOfValuesInUlong;
            }

            int remaining = count - processed;
            if (remaining > 0) {
                ulong packed = 0;
                for (int i = 0; i < remaining; i++) {
                    packed |= (ulong)values[processed + i] << (i * TypeBitSizes.UIntSize);
                }
                WriteBits(packed, remaining * TypeBitSizes.UIntSize);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteLong(long value) { WriteBits((ulong)value, TypeBitSizes.LongSize); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteLongs(ReadOnlySpan<long> values) {
            foreach (long value in values) { WriteBits((ulong)value, TypeBitSizes.LongSize); }
        }
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteULong(ulong value) { WriteBits(value, TypeBitSizes.ULongSize); }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteULongs(ReadOnlySpan<ulong> values) {
            foreach (ulong value in values) { WriteBits(value, TypeBitSizes.ULongSize); }
        }
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteFloat(float value) {
            uint asInt = (uint)BitConverter.SingleToInt32Bits(value);
            WriteBits(asInt, TypeBitSizes.FloatSize);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteFloats(ReadOnlySpan<float> values) {
            const int numberOfValuesInUlong = TypeBitSizes.ULongSize / TypeBitSizes.FloatSize;
            int count = values.Length;
            int processed = 0;
            
            while (processed + numberOfValuesInUlong <= count) {
                ulong packed = 0;
                for (int i = 0; i < numberOfValuesInUlong; i++) {
                    uint asInt = (uint)BitConverter.SingleToInt32Bits(values[processed + i]);
                    packed |= (ulong)asInt << (i * TypeBitSizes.FloatSize);
                }
                WriteBits(packed, TypeBitSizes.ULongSize);
                processed += numberOfValuesInUlong;
            }
            
            int remaining = count - processed;
            if (remaining > 0) {
                ulong packed = 0;
                for (int i = 0; i < remaining; i++) {
                    uint asInt = (uint)BitConverter.SingleToInt32Bits(values[processed + i]);
                    packed |= (ulong)asInt << (i * TypeBitSizes.FloatSize);
                }
                WriteBits(packed, remaining * TypeBitSizes.FloatSize);
            }
        }
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteDouble(double value) {
            ulong asLong = (ulong)BitConverter.DoubleToInt64Bits(value);
            WriteBits(asLong, TypeBitSizes.DoubleSize);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteDoubles(ReadOnlySpan<double> values) {
            foreach (double value in values) {
                ulong asLong = (ulong)BitConverter.DoubleToInt64Bits(value);
                WriteBits(asLong, TypeBitSizes.DoubleSize);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EnsureSpace(int bitsNeeded, string forWhat) {
            if (BitsRemaining < bitsNeeded) { throw new InsufficientWriteSpaceException(forWhat, bitsNeeded, BitsRemaining); }
        }
        
        public class InsufficientWriteSpaceException : Exception { public InsufficientWriteSpaceException(string failedThing, int requiredBits, int availableBits) : base($"Insufficient space to write {failedThing}. Required bits: {requiredBits}, Available bits: {availableBits}.") { } }
    }
}