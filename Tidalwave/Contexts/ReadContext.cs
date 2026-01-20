using System;
using System.Runtime.CompilerServices;

namespace Tidalwave {
    public ref partial struct ReadContext {
        private readonly ReadOnlySpan<ulong> buffer;
        private int bitPosition;
        private readonly int bitCapacity;

        public ReadContext(ReadOnlySpan<ulong> buffer) {
            this.buffer = buffer;
            bitPosition = 0;
            bitCapacity = buffer.Length * 64;
        }
        
        private int BitsRemaining => bitCapacity - bitPosition;
        
        private bool _PeakBit() {
            int ulongIndex = bitPosition / TypeBitSizes.ULongSize;
            int bitOffset = bitPosition % TypeBitSizes.ULongSize;
            return (buffer[ulongIndex] & (1UL << bitOffset)) != 0;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool _ReadBit() {
            bool value = _PeakBit();
            bitPosition++;
            return value;
        }
        
        private ulong _PeakBits(int count) {
            int ulongIndex = bitPosition / TypeBitSizes.ULongSize;
            int bitOffset = bitPosition % TypeBitSizes.ULongSize;

            ulong valueMask = count == TypeBitSizes.ULongSize ? ulong.MaxValue : (1UL << count) - 1;
            ulong result = (buffer[ulongIndex] >> bitOffset);

            int bitsAvailableInCurrent = TypeBitSizes.ULongSize - bitOffset;
            if (count > bitsAvailableInCurrent) { result |= (buffer[ulongIndex + 1] << bitsAvailableInCurrent); }

            return result & valueMask;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ulong _ReadBits(int count) {
            ulong value = _PeakBits(count);
            bitPosition += count;
            return value;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool _PeakBool() { return _PeakBit(); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool _ReadBool() {
            bool value = _PeakBool();
            bitPosition += TypeBitSizes.BoolSize;
            return value;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool[] _PeakBools(int count) {
            bool[] result = new bool[count];
            for (int i = 0; i < count; i++) { result[i] = _PeakBool(); }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool[] _ReadBools(int count) {
            bool[] values = _PeakBools(count);
            bitPosition += count * TypeBitSizes.BoolSize;
            return values;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private byte _PeakByte() { return (byte)_PeakBits(TypeBitSizes.ByteSize); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private byte _ReadByte() {
            byte value = _PeakByte();
            bitPosition += TypeBitSizes.ByteSize;
            return value;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private byte[] _PeakBytes(int count) {
            byte[] result = new byte[count];
            for (int i = 0; i < count; i++) { result[i] = _PeakByte(); }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private byte[] _ReadBytes(int count) {
            byte[] values = _PeakBytes(count);
            bitPosition += count * TypeBitSizes.ByteSize;
            return values;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private sbyte _PeakSByte() { return (sbyte)_PeakBits(TypeBitSizes.SByteSize); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private sbyte _ReadSByte() {
            sbyte value = _PeakSByte();
            bitPosition += TypeBitSizes.SByteSize;
            return value;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private sbyte[] _PeakSBytes(int count) {
            sbyte[] result = new sbyte[count];
            for (int i = 0; i < count; i++) { result[i] = _PeakSByte(); }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private sbyte[] _ReadSBytes(int count) {
            sbyte[] values = _PeakSBytes(count);
            bitPosition += count * TypeBitSizes.SByteSize;
            return values;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private short _PeakShort() { return (short)_PeakBits(TypeBitSizes.ShortSize); }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private short _ReadShort() {
            short value = _PeakShort();
            bitPosition += TypeBitSizes.ShortSize;
            return value;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private short[] _PeakShorts(int count) {
            short[] result = new short[count];
            for (int i = 0; i < count; i++) { result[i] = _PeakShort(); }
            return result;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private short[] _ReadShorts(int count) {
            short[] values = _PeakShorts(count);
            bitPosition += count * TypeBitSizes.ShortSize;
            return values;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ushort _PeakUShort() { return (ushort)_PeakBits(TypeBitSizes.UShortSize); }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ushort _ReadUShort() {
            ushort value = _PeakUShort();
            bitPosition += TypeBitSizes.UShortSize;
            return value;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ushort[] _PeakUShorts(int count) {
            ushort[] result = new ushort[count];
            for (int i = 0; i < count; i++) { result[i] = _PeakUShort(); }
            return result;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ushort[] _ReadUShorts(int count) {
            ushort[] values = _PeakUShorts(count);
            bitPosition += count * TypeBitSizes.UShortSize;
            return values;
        }
       
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int _PeakInt() { return (int)_PeakBits(TypeBitSizes.IntSize); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int _ReadInt() {
            int value = _PeakInt();
            bitPosition += TypeBitSizes.IntSize;
            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int[] _PeakInts(int count) {
            int[] result = new int[count];
            for (int i = 0; i < count; i++) { result[i] = _PeakInt(); }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int[] _ReadInts(int count) {
            int[] values = _PeakInts(count);
            bitPosition += count * TypeBitSizes.IntSize;
            return values;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private uint _PeakUInt() { return (uint)_PeakBits(TypeBitSizes.UIntSize); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private uint _ReadUInt() {
            uint value = _PeakUInt();
            bitPosition += TypeBitSizes.UIntSize;
            return value;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private uint[] _PeakUInts(int count) {
            uint[] result = new uint[count];
            for (int i = 0; i < count; i++) { result[i] = _PeakUInt(); }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private uint[] _ReadUInts(int count) {
            uint[] values = _PeakUInts(count);
            bitPosition += count * TypeBitSizes.UIntSize;
            return values;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private long _PeakLong() { return (long)_PeakBits(TypeBitSizes.LongSize); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private long _ReadLong() {
            long value = _PeakLong();
            bitPosition += TypeBitSizes.LongSize;
            return value;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private long[] _PeakLongs(int count) {
            long[] result = new long[count];
            for (int i = 0; i < count; i++) { result[i] = _PeakLong(); }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private long[] _ReadLongs(int count) {
            long[] values = _PeakLongs(count);
            bitPosition += count * TypeBitSizes.LongSize;
            return values;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ulong _PeakULong() { return _PeakBits(TypeBitSizes.ULongSize); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ulong _ReadULong() {
            ulong value = _PeakULong();
            bitPosition += TypeBitSizes.ULongSize;
            return value;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ulong[] _PeakULongs(int count) {
            ulong[] result = new ulong[count];
            for (int i = 0; i < count; i++) { result[i] = _PeakULong(); }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ulong[] _ReadULongs(int count) {
            ulong[] values = _PeakULongs(count);
            bitPosition += count * TypeBitSizes.ULongSize;
            return values;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private float _PeakFloat() {
            uint intValue = (uint)_PeakBits(TypeBitSizes.FloatSize);
            return BitConverter.Int32BitsToSingle((int)intValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private float _ReadFloat() {
            float value = _PeakFloat();
            bitPosition += TypeBitSizes.FloatSize;
            return value;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private float[] _PeakFloats(int count) {
            float[] result = new float[count];
            for (int i = 0; i < count; i++) { result[i] = _PeakFloat(); }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private float[] _ReadFloats(int count) {
            float[] values = _PeakFloats(count);
            bitPosition += count * TypeBitSizes.FloatSize;
            return values;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private double _PeakDouble() {
            ulong longValue = _PeakBits(TypeBitSizes.DoubleSize);
            return BitConverter.Int64BitsToDouble((long)longValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private double _ReadDouble() {
            double value = _PeakDouble();
            bitPosition += TypeBitSizes.DoubleSize;
            return value;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private double[] _PeakDoubles(int count) {
            double[] result = new double[count];
            for (int i = 0; i < count; i++) { result[i] = _PeakDouble(); }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private double[] _ReadDoubles(int count) {
            double[] values = _PeakDoubles(count);
            bitPosition += count * TypeBitSizes.DoubleSize;
            return values;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool CheckForSpace(int bitsNeeded) { return BitsRemaining >= bitsNeeded; }
    }
}