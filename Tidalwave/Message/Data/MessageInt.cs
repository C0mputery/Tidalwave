using System;
using Riptide.Utils;

namespace Riptide;

public partial class Message {
    /// <summary>Adds an <see cref="int"/> to the message.</summary>
    /// <param name="value">The <see cref="int"/> to add.</param>
    /// <returns>The message that the <see cref="int"/> was added to.</returns>
    public Message AddInt(int value) {
        if (UnwrittenBits < sizeof(int) * BitsPerByte) {
            throw new InsufficientCapacityException(this, IntName, sizeof(int) * BitsPerByte);
        }

        Converter.IntToBits(value, Data, WriteBit);
        WriteBit += sizeof(int) * BitsPerByte;
        return this;
    }

    /// <summary>Adds a <see cref="uint"/> to the message.</summary>
    /// <param name="value">The <see cref="uint"/> to add.</param>
    /// <returns>The message that the <see cref="uint"/> was added to.</returns>
    public Message AddUInt(uint value) {
        if (UnwrittenBits < sizeof(uint) * BitsPerByte) {
            throw new InsufficientCapacityException(this, UIntName, sizeof(uint) * BitsPerByte);
        }

        Converter.UIntToBits(value, Data, WriteBit);
        WriteBit += sizeof(uint) * BitsPerByte;
        return this;
    }

    /// <summary>Retrieves an <see cref="int"/> from the message.</summary>
    /// <returns>The <see cref="int"/> that was retrieved.</returns>
    public int GetInt() {
        if (UnreadBits < sizeof(int) * BitsPerByte) {
            RiptideLogger.Log(LogType.Error, NotEnoughBitsError(IntName, $"{default(int)}"));
            return default;
        }

        int value = Converter.IntFromBits(Data, ReadBit);
        ReadBit += sizeof(int) * BitsPerByte;
        return value;
    }

    /// <summary>Retrieves a <see cref="uint"/> from the message.</summary>
    /// <returns>The <see cref="uint"/> that was retrieved.</returns>
    public uint GetUInt() {
        if (UnreadBits < sizeof(uint) * BitsPerByte) {
            RiptideLogger.Log(LogType.Error, NotEnoughBitsError(UIntName, $"{default(uint)}"));
            return default;
        }

        uint value = Converter.UIntFromBits(Data, ReadBit);
        ReadBit += sizeof(uint) * BitsPerByte;
        return value;
    }

    /// <summary>Adds an <see cref="int"/> array message.</summary>
    /// <param name="array">The array to add.</param>
    /// <param name="includeLength">Whether or not to include the length of the array in the message.</param>
    /// <returns>The message that the array was added to.</returns>
    public Message AddInts(int[] array, bool includeLength = true) {
        if (includeLength) {
            AddVarULong((uint)array.Length);
        }

        if (UnwrittenBits < array.Length * sizeof(int) * BitsPerByte) {
            throw new InsufficientCapacityException(this, array.Length, IntName, sizeof(int) * BitsPerByte);
        }

        for (int i = 0; i < array.Length; i++) {
            Converter.IntToBits(array[i], Data, WriteBit);
            WriteBit += sizeof(int) * BitsPerByte;
        }

        return this;
    }

    /// <summary>Adds a <see cref="uint"/> array to the message.</summary>
    /// <param name="array">The array to add.</param>
    /// <param name="includeLength">Whether or not to include the length of the array in the message.</param>
    /// <returns>The message that the array was added to.</returns>
    public Message AddUInts(uint[] array, bool includeLength = true) {
        if (includeLength) {
            AddVarULong((uint)array.Length);
        }

        if (UnwrittenBits < array.Length * sizeof(uint) * BitsPerByte) {
            throw new InsufficientCapacityException(this, array.Length, UIntName, sizeof(uint) * BitsPerByte);
        }

        for (int i = 0; i < array.Length; i++) {
            Converter.UIntToBits(array[i], Data, WriteBit);
            WriteBit += sizeof(uint) * BitsPerByte;
        }

        return this;
    }

    /// <summary>Retrieves an <see cref="int"/> array from the message.</summary>
    /// <returns>The array that was retrieved.</returns>
    public int[] GetInts() {
        return GetInts((int)GetVarULong());
    }

    /// <summary>Retrieves an <see cref="int"/> array from the message.</summary>
    /// <param name="amount">The amount of ints to retrieve.</param>
    /// <returns>The array that was retrieved.</returns>
    public int[] GetInts(int amount) {
        int[] array = new int[amount];
        ReadInts(amount, array);
        return array;
    }

    /// <summary>Populates an <see cref="int"/> array with ints retrieved from the message.</summary>
    /// <param name="intoArray">The array to populate.</param>
    /// <param name="startIndex">The position at which to start populating the array.</param>
    public void GetInts(int[] intoArray, int startIndex = 0) {
        GetInts((int)GetVarULong(), intoArray, startIndex);
    }

    /// <summary>Populates an <see cref="int"/> array with ints retrieved from the message.</summary>
    /// <param name="amount">The amount of ints to retrieve.</param>
    /// <param name="intoArray">The array to populate.</param>
    /// <param name="startIndex">The position at which to start populating the array.</param>
    public void GetInts(int amount, int[] intoArray, int startIndex = 0) {
        if (startIndex + amount > intoArray.Length) {
            throw new ArgumentException(nameof(amount), ArrayNotLongEnoughError(amount, intoArray.Length, startIndex, IntName));
        }

        ReadInts(amount, intoArray, startIndex);
    }

    /// <summary>Retrieves a <see cref="uint"/> array from the message.</summary>
    /// <returns>The array that was retrieved.</returns>
    public uint[] GetUInts() {
        return GetUInts((int)GetVarULong());
    }

    /// <summary>Retrieves a <see cref="uint"/> array from the message.</summary>
    /// <param name="amount">The amount of uints to retrieve.</param>
    /// <returns>The array that was retrieved.</returns>
    public uint[] GetUInts(int amount) {
        uint[] array = new uint[amount];
        ReadUInts(amount, array);
        return array;
    }

    /// <summary>Populates a <see cref="uint"/> array with uints retrieved from the message.</summary>
    /// <param name="intoArray">The array to populate.</param>
    /// <param name="startIndex">The position at which to start populating the array.</param>
    public void GetUInts(uint[] intoArray, int startIndex = 0) {
        GetUInts((int)GetVarULong(), intoArray, startIndex);
    }

    /// <summary>Populates a <see cref="uint"/> array with uints retrieved from the message.</summary>
    /// <param name="amount">The amount of uints to retrieve.</param>
    /// <param name="intoArray">The array to populate.</param>
    /// <param name="startIndex">The position at which to start populating the array.</param>
    public void GetUInts(int amount, uint[] intoArray, int startIndex = 0) {
        if (startIndex + amount > intoArray.Length) {
            throw new ArgumentException(nameof(amount), ArrayNotLongEnoughError(amount, intoArray.Length, startIndex, UIntName));
        }

        ReadUInts(amount, intoArray, startIndex);
    }

    /// <summary>Reads a number of ints from the message and writes them into the given array.</summary>
    /// <param name="amount">The amount of ints to read.</param>
    /// <param name="intoArray">The array to write the ints into.</param>
    /// <param name="startIndex">The position at which to start writing into the array.</param>
    private void ReadInts(int amount, int[] intoArray, int startIndex = 0) {
        if (UnreadBits < amount * sizeof(int) * BitsPerByte) {
            RiptideLogger.Log(LogType.Error, NotEnoughBitsError(amount, IntName));
            amount = UnreadBits / (sizeof(int) * BitsPerByte);
        }

        for (int i = 0; i < amount; i++) {
            intoArray[startIndex + i] = Converter.IntFromBits(Data, ReadBit);
            ReadBit += sizeof(int) * BitsPerByte;
        }
    }

    /// <summary>Reads a number of uints from the message and writes them into the given array.</summary>
    /// <param name="amount">The amount of uints to read.</param>
    /// <param name="intoArray">The array to write the uints into.</param>
    /// <param name="startIndex">The position at which to start writing into the array.</param>
    private void ReadUInts(int amount, uint[] intoArray, int startIndex = 0) {
        if (UnreadBits < amount * sizeof(uint) * BitsPerByte) {
            RiptideLogger.Log(LogType.Error, NotEnoughBitsError(amount, UIntName));
            amount = UnreadBits / (sizeof(uint) * BitsPerByte);
        }

        for (int i = 0; i < amount; i++) {
            intoArray[startIndex + i] = Converter.UIntFromBits(Data, ReadBit);
            ReadBit += sizeof(uint) * BitsPerByte;
        }
    }
}