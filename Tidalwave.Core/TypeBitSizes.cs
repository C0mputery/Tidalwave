namespace Tidalwave {
    public static class TypeBitSizes {
        public const int BoolSize = 1;
        public const int ByteSize = 8;
        public const int SByteSize = sizeof(sbyte) * ByteSize;
        public const int ShortSize = sizeof(short) * ByteSize;
        public const int UShortSize = sizeof(ushort) * ByteSize;
        public const int IntSize = sizeof(int) * ByteSize;
        public const int UIntSize = sizeof(uint) * ByteSize;
        public const int LongSize = sizeof(long) * ByteSize;
        public const int ULongSize = sizeof(ulong) * ByteSize;
        public const int FloatSize = sizeof(float) * ByteSize;
        public const int DoubleSize = sizeof(double) * ByteSize;
    }
}