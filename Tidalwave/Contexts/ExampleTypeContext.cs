/*using System;

using System.Runtime.CompilerServices;

namespace Tidalwave {
    public ref partial struct WriteContext {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteBool(bool value) {
            ThrowIfNoSpace(TypeBitSizes.BoolSize, "bool");
            _WriteBool(value);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(bool value) {
            ThrowIfNoSpace(TypeBitSizes.BoolSize, "bool");
            _WriteBool(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteBools(ReadOnlySpan<bool> values) {
            int bitsNeeded = values.Length * TypeBitSizes.BoolSize + TypeBitSizes.IntSize;
            ThrowIfNoSpace(bitsNeeded, "bool array");
            _WriteInt(values.Length);
            _WriteBools(values);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(Span<bool> values) {
            int bitsNeeded = values.Length * TypeBitSizes.BoolSize + TypeBitSizes.IntSize;
            ThrowIfNoSpace(bitsNeeded, "bool array");
            _WriteInt(values.Length);
            _WriteBools(values);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteBoolsWithoutLength(ReadOnlySpan<bool> values) {
            int bitsNeeded = values.Length * TypeBitSizes.BoolSize;
            ThrowIfNoSpace(bitsNeeded, "bool array without length");
            _WriteBools(values);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteWithoutLength(Span<bool> values) {
            int bitsNeeded = values.Length * TypeBitSizes.BoolSize + TypeBitSizes.IntSize;
            ThrowIfNoSpace(bitsNeeded, "bool array");
            _WriteInt(values.Length);
            _WriteBools(values);
        }
    }
    
    
    public ref partial struct ReadContext {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ReadBool() {
            if (!CheckForSpace(TypeBitSizes.BoolSize)) { return default; }
            return _ReadBool();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryReadBool(out bool value) {
            if (!CheckForSpace(TypeBitSizes.BoolSize)) {
                value = default;
                return false;
            }
            value = _ReadBool();
            return true;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool[] ReadBools() {
            if (!CheckForSpace(TypeBitSizes.IntSize)) { return Array.Empty<bool>(); }
            int peakedLength = _PeakInt();
            int bitsNeeded = peakedLength * TypeBitSizes.BoolSize + TypeBitSizes.IntSize;
            return !CheckForSpace(bitsNeeded) ? Array.Empty<bool>() : _ReadBools(peakedLength);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryReadBools(out bool[] values) {
            if (!CheckForSpace(TypeBitSizes.IntSize)) {
                values = Array.Empty<bool>();
                return false;
            }
            int peakedLength = _PeakInt();
            int bitsNeeded = peakedLength * TypeBitSizes.BoolSize + TypeBitSizes.IntSize;
            if (!CheckForSpace(bitsNeeded)) {
                values = Array.Empty<bool>();
                return false;
            }
            values = _ReadBools(peakedLength);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool[] ReadBools(int count) {
            int bitsNeeded = count * TypeBitSizes.BoolSize;
            return !CheckForSpace(bitsNeeded) ? Array.Empty<bool>() : _ReadBools(count);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryReadBools(int count, out bool[] values) {
            int bitsNeeded = count * TypeBitSizes.BoolSize;
            if (!CheckForSpace(bitsNeeded)) {
                values = Array.Empty<bool>();
                return false;
            }
            values = _ReadBools(count);
            return true;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool PeakBool() {
            if (!CheckForSpace(TypeBitSizes.BoolSize)) { return default; }
            return _PeakBool();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryPeakBool(out bool value) {
            if (!CheckForSpace(TypeBitSizes.BoolSize)) {
                value = default;
                return false;
            }

            value = _PeakBool();
            return true;
        }
    }
}
*/