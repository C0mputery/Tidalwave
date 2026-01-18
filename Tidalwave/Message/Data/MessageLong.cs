using System;
using Riptide.Utils;

namespace Riptide;

public partial class Message {
    /// <summary>Adds a <see cref="long"/> to the message.</summary>
    /// <param name="value">The <see cref="long"/> to add.</param>
    /// <returns>The message that the <see cref="long"/> was added to.</returns>
    public Message AddLong(long value) {
        if (UnwrittenBits < sizeof(long) * BitsPerByte) {
            throw new InsufficientCapacityException(this, LongName, sizeof(long) * BitsPerByte);
        }

        Converter.LongToBits(value, Data, WriteBit);
        WriteBit += sizeof(long) * BitsPerByte;
        return this;
    }

    /// <summary>Adds a <see cref="ulong"/> to the message.</summary>
    /// <param name="value">The <see cref="ulong"/> to add.</param>
    /// <returns>The message that the <see cref="ulong"/> was added to.</returns>
    public Message AddULong(ulong value) {
        if (UnwrittenBits < sizeof(ulong) * BitsPerByte) {
            throw new InsufficientCapacityException(this, ULongName, sizeof(ulong) * BitsPerByte);
        }

        Converter.ULongToBits(value, Data, WriteBit);
        WriteBit += sizeof(ulong) * BitsPerByte;
        return this;
    }

    /// <summary>Retrieves a <see cref="long"/> from the message.</summary>
    /// <returns>The <see cref="long"/> that was retrieved.</returns>
    public long GetLong() {
        if (UnreadBits < sizeof(long) * BitsPerByte) {
            RiptideLogger.Log(LogType.Error, NotEnoughBitsError(LongName, $"{default(long)}"));
            return default;
        }

        long value = Converter.LongFromBits(Data, ReadBit);
        ReadBit += sizeof(long) * BitsPerByte;
        return value;
    }

    /// <summary>Retrieves a <see cref="ulong"/> from the message.</summary>
    /// <returns>The <see cref="ulong"/> that was retrieved.</returns>
    public ulong GetULong() {
        if (UnreadBits < sizeof(ulong) * BitsPerByte) {
            RiptideLogger.Log(LogType.Error, NotEnoughBitsError(ULongName, $"{default(ulong)}"));
            return default;
        }

        ulong value = Converter.ULongFromBits(Data, ReadBit);
        ReadBit += sizeof(ulong) * BitsPerByte;
        return value;
    }

    /// <summary>Adds a <see cref="long"/> array to the message.</summary>
    /// <param name="array">The array to add.</param>
    /// <param name="includeLength">Whether or not to include the length of the array in the message.</param>
    /// <returns>The message that the array was added to.</returns>
    public Message AddLongs(long[] array, bool includeLength = true) {
        if (includeLength) {
            AddVarULong((uint)array.Length);
        }

        if (UnwrittenBits < array.Length * sizeof(long) * BitsPerByte) {
            throw new InsufficientCapacityException(this, array.Length, LongName, sizeof(long) * BitsPerByte);
        }

        for (int i = 0; i < array.Length; i++) {
            Converter.LongToBits(array[i], Data, WriteBit);
            WriteBit += sizeof(long) * BitsPerByte;
        }

        return this;
    }

    /// <summary>Adds a <see cref="ulong"/> array to the message.</summary>
    /// <param name="array">The array to add.</param>
    /// <param name="includeLength">Whether or not to include the length of the array in the message.</param>
    /// <returns>The message that the array was added to.</returns>
    public Message AddULongs(ulong[] array, bool includeLength = true) {
        if (includeLength) {
            AddVarULong((uint)array.Length);
        }

        if (UnwrittenBits < array.Length * sizeof(ulong) * BitsPerByte) {
            throw new InsufficientCapacityException(this, array.Length, ULongName, sizeof(ulong) * BitsPerByte);
        }

        for (int i = 0; i < array.Length; i++) {
            Converter.ULongToBits(array[i], Data, WriteBit);
            WriteBit += sizeof(ulong) * BitsPerByte;
        }

        return this;
    }

    /// <summary>Retrieves a <see cref="long"/> array from the message.</summary>
    /// <returns>The array that was retrieved.</returns>
    public long[] GetLongs() {
        return GetLongs((int)GetVarULong());
    }

    /// <summary>Retrieves a <see cref="long"/> array from the message.</summary>
    /// <param name="amount">The amount of longs to retrieve.</param>
    /// <returns>The array that was retrieved.</returns>
    public long[] GetLongs(int amount) {
        long[] array = new long[amount];
        ReadLongs(amount, array);
        return array;
    }

    /// <summary>Populates a <see cref="long"/> array with longs retrieved from the message.</summary>
    /// <param name="intoArray">The array to populate.</param>
    /// <param name="startIndex">The position at which to start populating the array.</param>
    public void GetLongs(long[] intoArray, int startIndex = 0) {
        GetLongs((int)GetVarULong(), intoArray, startIndex);
    }

    /// <summary>Populates a <see cref="long"/> array with longs retrieved from the message.</summary>
    /// <param name="amount">The amount of longs to retrieve.</param>
    /// <param name="intoArray">The array to populate.</param>
    /// <param name="startIndex">The position at which to start populating the array.</param>
    public void GetLongs(int amount, long[] intoArray, int startIndex = 0) {
        if (startIndex + amount > intoArray.Length) {
            throw new ArgumentException(nameof(amount), ArrayNotLongEnoughError(amount, intoArray.Length, startIndex, LongName));
        }

        ReadLongs(amount, intoArray, startIndex);
    }

    /// <summary>Retrieves a <see cref="ulong"/> array from the message.</summary>
    /// <returns>The array that was retrieved.</returns>
    public ulong[] GetULongs() {
        return GetULongs((int)GetVarULong());
    }

    /// <summary>Retrieves a <see cref="ulong"/> array from the message.</summary>
    /// <param name="amount">The amount of ulongs to retrieve.</param>
    /// <returns>The array that was retrieved.</returns>
    public ulong[] GetULongs(int amount) {
        ulong[] array = new ulong[amount];
        ReadULongs(amount, array);
        return array;
    }

    /// <summary>Populates a <see cref="ulong"/> array with ulongs retrieved from the message.</summary>
    /// <param name="intoArray">The array to populate.</param>
    /// <param name="startIndex">The position at which to start populating the array.</param>
    public void GetULongs(ulong[] intoArray, int startIndex = 0) {
        GetULongs((int)GetVarULong(), intoArray, startIndex);
    }

    /// <summary>Populates a <see cref="ulong"/> array with ulongs retrieved from the message.</summary>
    /// <param name="amount">The amount of ulongs to retrieve.</param>
    /// <param name="intoArray">The array to populate.</param>
    /// <param name="startIndex">The position at which to start populating the array.</param>
    public void GetULongs(int amount, ulong[] intoArray, int startIndex = 0) {
        if (startIndex + amount > intoArray.Length) {
            throw new ArgumentException(nameof(amount), ArrayNotLongEnoughError(amount, intoArray.Length, startIndex, ULongName));
        }

        ReadULongs(amount, intoArray, startIndex);
    }

    /// <summary>Reads a number of longs from the message and writes them into the given array.</summary>
    /// <param name="amount">The amount of longs to read.</param>
    /// <param name="intoArray">The array to write the longs into.</param>
    /// <param name="startIndex">The position at which to start writing into the array.</param>
    private void ReadLongs(int amount, long[] intoArray, int startIndex = 0) {
        if (UnreadBits < amount * sizeof(long) * BitsPerByte) {
            RiptideLogger.Log(LogType.Error, NotEnoughBitsError(amount, LongName));
            amount = UnreadBits / (sizeof(long) * BitsPerByte);
        }

        for (int i = 0; i < amount; i++) {
            intoArray[startIndex + i] = Converter.LongFromBits(Data, ReadBit);
            ReadBit += sizeof(long) * BitsPerByte;
        }
    }

    /// <summary>Reads a number of ulongs from the message and writes them into the given array.</summary>
    /// <param name="amount">The amount of ulongs to read.</param>
    /// <param name="intoArray">The array to write the ulongs into.</param>
    /// <param name="startIndex">The position at which to start writing into the array.</param>
    private void ReadULongs(int amount, ulong[] intoArray, int startIndex = 0) {
        if (UnreadBits < amount * sizeof(ulong) * BitsPerByte) {
            RiptideLogger.Log(LogType.Error, NotEnoughBitsError(amount, ULongName));
            amount = UnreadBits / (sizeof(ulong) * BitsPerByte);
        }

        for (int i = 0; i < amount; i++) {
            intoArray[startIndex + i] = Converter.ULongFromBits(Data, ReadBit);
            ReadBit += sizeof(ulong) * BitsPerByte;
        }
    }
}