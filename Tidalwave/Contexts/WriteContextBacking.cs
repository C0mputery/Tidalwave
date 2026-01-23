using System;
using System.Runtime.CompilerServices;

namespace Tidalwave {
    public ref partial struct WriteContext {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void _WriteBool(bool value) { WriteBit(value); }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void _WriteBools(ReadOnlySpan<bool> values) {
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
        private void _WriteByte(byte value) { WriteBits(value, TypeBitSizes.ByteSize); }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void _WriteBytes(ReadOnlySpan<byte> values) {
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
        private void _WriteSByte(sbyte value) { WriteBits((byte)value, TypeBitSizes.SByteSize); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void _WriteSBytes(ReadOnlySpan<sbyte> values) {
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
        private void _WriteShort(short value) { WriteBits((ushort)value, TypeBitSizes.ShortSize); }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void _WriteShorts(ReadOnlySpan<short> values) {
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
        private void _WriteUShort(ushort value) { WriteBits(value, TypeBitSizes.UShortSize); }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void _WriteUShorts(ReadOnlySpan<ushort> values) {
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
        private void _WriteInt(int value) { WriteBits((uint)value, TypeBitSizes.IntSize); }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void _WriteInts(ReadOnlySpan<int> values) {
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
        private void _WriteUInt(uint value) { WriteBits(value, TypeBitSizes.UIntSize); }
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void _WriteUInts(ReadOnlySpan<uint> values) {
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
        private void _WriteLong(long value) { WriteBits((ulong)value, TypeBitSizes.LongSize); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void _WriteLongs(ReadOnlySpan<long> values) {
            foreach (long value in values) { WriteBits((ulong)value, TypeBitSizes.LongSize); }
        }
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void _WriteULong(ulong value) { WriteBits(value, TypeBitSizes.ULongSize); }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void _WriteULongs(ReadOnlySpan<ulong> values) {
            foreach (ulong value in values) { WriteBits(value, TypeBitSizes.ULongSize); }
        }
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void _WriteFloat(float value) {
            uint asInt = (uint)BitConverter.SingleToInt32Bits(value);
            WriteBits(asInt, TypeBitSizes.FloatSize);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void _WriteFloats(ReadOnlySpan<float> values) {
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
        private void _WriteDouble(double value) {
            ulong asLong = (ulong)BitConverter.DoubleToInt64Bits(value);
            WriteBits(asLong, TypeBitSizes.DoubleSize);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void _WriteDoubles(ReadOnlySpan<double> values) {
            foreach (double value in values) {
                ulong asLong = (ulong)BitConverter.DoubleToInt64Bits(value);
                WriteBits(asLong, TypeBitSizes.DoubleSize);
            }
        }
    }
}