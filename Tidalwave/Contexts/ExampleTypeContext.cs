/*using System;
using System.Runtime.CompilerServices;

namespace Tidalwave {
    public ref partial struct WriteContext {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddBool(bool value) {
            EnsureSpace(TypeBitSizes.BoolSize, "bool");
            WriteBool(value);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(bool value) {
            EnsureSpace(TypeBitSizes.BoolSize, "bool");
            WriteBool(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddBools(ReadOnlySpan<bool> values) {
            int bitsNeeded = values.Length * TypeBitSizes.BoolSize + TypeBitSizes.IntSize;
            EnsureSpace(bitsNeeded, "bool array");
            WriteInt(values.Length);
            WriteBools(values);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(Span<bool> values) {
            int bitsNeeded = values.Length * TypeBitSizes.BoolSize + TypeBitSizes.IntSize;
            EnsureSpace(bitsNeeded, "bool array");
            WriteInt(values.Length);
            WriteBools(values);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddBoolsWithoutLength(ReadOnlySpan<bool> values) {
            int bitsNeeded = values.Length * TypeBitSizes.BoolSize;
            EnsureSpace(bitsNeeded, "bool array without length");
            WriteBools(values);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddWithoutLength(Span<bool> values) {
            int bitsNeeded = values.Length * TypeBitSizes.BoolSize + TypeBitSizes.IntSize;
            EnsureSpace(bitsNeeded, "bool array");
            WriteInt(values.Length);
            WriteBools(values);
        }
    }
}
*/