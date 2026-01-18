using System.Runtime.CompilerServices;

namespace Riptide;

public partial class Message {
    /// <inheritdoc cref="AddByte(byte)"/>
    /// <remarks>This method is simply an alternative way of calling <see cref="AddByte(byte)"/>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Message Add(byte value) { return AddByte(value); }

    /// <inheritdoc cref="AddSByte(sbyte)"/>
    /// <remarks>This method is simply an alternative way of calling <see cref="AddSByte(sbyte)"/>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Message Add(sbyte value) { return AddSByte(value); }

    /// <inheritdoc cref="AddBool(bool)"/>
    /// <remarks>This method is simply an alternative way of calling <see cref="AddBool(bool)"/>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Message Add(bool value) { return AddBool(value); }

    /// <inheritdoc cref="AddShort(short)"/>
    /// <remarks>This method is simply an alternative way of calling <see cref="AddShort(short)"/>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Message Add(short value) { return AddShort(value); }

    /// <inheritdoc cref="AddUShort(ushort)"/>
    /// <remarks>This method is simply an alternative way of calling <see cref="AddUShort(ushort)"/>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Message Add(ushort value) { return AddUShort(value); }

    /// <inheritdoc cref="AddInt(int)"/>
    /// <remarks>This method is simply an alternative way of calling <see cref="AddInt(int)"/>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Message Add(int value) { return AddInt(value); }

    /// <inheritdoc cref="AddUInt(uint)"/>
    /// <remarks>This method is simply an alternative way of calling <see cref="AddUInt(uint)"/>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Message Add(uint value) { return AddUInt(value); }

    /// <inheritdoc cref="AddLong(long)"/>
    /// <remarks>This method is simply an alternative way of calling <see cref="AddLong(long)"/>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Message Add(long value) { return AddLong(value); }

    /// <inheritdoc cref="AddULong(ulong)"/>
    /// <remarks>This method is simply an alternative way of calling <see cref="AddULong(ulong)"/>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Message Add(ulong value) { return AddULong(value); }

    /// <inheritdoc cref="AddFloat(float)"/>
    /// <remarks>This method is simply an alternative way of calling <see cref="AddFloat(float)"/>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Message Add(float value) { return AddFloat(value); }

    /// <inheritdoc cref="AddDouble(double)"/>
    /// <remarks>This method is simply an alternative way of calling <see cref="AddDouble(double)"/>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Message Add(double value) { return AddDouble(value); }

    /// <inheritdoc cref="AddString(string)"/>
    /// <remarks>This method is simply an alternative way of calling <see cref="AddString(string)"/>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Message Add(string value) { return AddString(value); }
    
    /// <inheritdoc cref="AddBytes(byte[], bool)"/>
    /// <remarks>This method is simply an alternative way of calling <see cref="AddBytes(byte[], bool)"/>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Message Add(byte[] array, bool includeLength = true) { return AddBytes(array, includeLength); }

    /// <inheritdoc cref="AddSBytes(sbyte[], bool)"/>
    /// <remarks>This method is simply an alternative way of calling <see cref="AddSBytes(sbyte[], bool)"/>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Message Add(sbyte[] array, bool includeLength = true) { return AddSBytes(array, includeLength); }

    /// <inheritdoc cref="AddBools(bool[], bool)"/>
    /// <remarks>This method is simply an alternative way of calling <see cref="AddBools(bool[], bool)"/>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Message Add(bool[] array, bool includeLength = true) { return AddBools(array, includeLength); }

    /// <inheritdoc cref="AddShorts(short[], bool)"/>
    /// <remarks>This method is simply an alternative way of calling <see cref="AddShorts(short[], bool)"/>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Message Add(short[] array, bool includeLength = true) { return AddShorts(array, includeLength); }

    /// <inheritdoc cref="AddUShorts(ushort[], bool)"/>
    /// <remarks>This method is simply an alternative way of calling <see cref="AddUShorts(ushort[], bool)"/>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Message Add(ushort[] array, bool includeLength = true) { return AddUShorts(array, includeLength); }

    /// <inheritdoc cref="AddInts(int[], bool)"/>
    /// <remarks>This method is simply an alternative way of calling <see cref="AddInts(int[], bool)"/>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Message Add(int[] array, bool includeLength = true) { return AddInts(array, includeLength); }

    /// <inheritdoc cref="AddUInts(uint[], bool)"/>
    /// <remarks>This method is simply an alternative way of calling <see cref="AddUInts(uint[], bool)"/>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Message Add(uint[] array, bool includeLength = true) { return AddUInts(array, includeLength); }

    /// <inheritdoc cref="AddLongs(long[], bool)"/>
    /// <remarks>This method is simply an alternative way of calling <see cref="AddLongs(long[], bool)"/>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Message Add(long[] array, bool includeLength = true) { return AddLongs(array, includeLength); }

    /// <inheritdoc cref="AddULongs(ulong[], bool)"/>
    /// <remarks>This method is simply an alternative way of calling <see cref="AddULongs(ulong[], bool)"/>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Message Add(ulong[] array, bool includeLength = true) { return AddULongs(array, includeLength); }

    /// <inheritdoc cref="AddFloats(float[], bool)"/>
    /// <remarks>This method is simply an alternative way of calling <see cref="AddFloats(float[], bool)"/>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Message Add(float[] array, bool includeLength = true) { return AddFloats(array, includeLength); }

    /// <inheritdoc cref="AddDoubles(double[], bool)"/>
    /// <remarks>This method is simply an alternative way of calling <see cref="AddDoubles(double[], bool)"/>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Message Add(double[] array, bool includeLength = true) { return AddDoubles(array, includeLength); }

    /// <inheritdoc cref="AddStrings(string[], bool)"/>
    /// <remarks>This method is simply an alternative way of calling <see cref="AddStrings(string[], bool)"/>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Message Add(string[] array, bool includeLength = true) { return AddStrings(array, includeLength); }
}