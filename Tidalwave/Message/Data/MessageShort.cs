using System;
using Riptide.Utils;

namespace Riptide;

public partial class Message {
    /// <summary>Adds a <see cref="short"/> to the message.</summary>
    /// <param name="value">The <see cref="short"/> to add.</param>
    /// <returns>The message that the <see cref="short"/> was added to.</returns>
    public Message AddShort(short value) {
        if (UnwrittenBits < sizeof(short) * BitsPerByte) {
            throw new InsufficientCapacityException(this, ShortName, sizeof(short) * BitsPerByte);
        }

        Converter.ShortToBits(value, Data, WriteBit);
        WriteBit += sizeof(short) * BitsPerByte;
        return this;
    }

    /// <summary>Adds a <see cref="ushort"/> to the message.</summary>
    /// <param name="value">The <see cref="ushort"/> to add.</param>
    /// <returns>The message that the <see cref="ushort"/> was added to.</returns>
    public Message AddUShort(ushort value) {
        if (UnwrittenBits < sizeof(ushort) * BitsPerByte) {
            throw new InsufficientCapacityException(this, UShortName, sizeof(ushort) * BitsPerByte);
        }

        Converter.UShortToBits(value, Data, WriteBit);
        WriteBit += sizeof(ushort) * BitsPerByte;
        return this;
    }

    /// <summary>Retrieves a <see cref="short"/> from the message.</summary>
    /// <returns>The <see cref="short"/> that was retrieved.</returns>
    public short GetShort() {
        if (UnreadBits < sizeof(short) * BitsPerByte) {
            RiptideLogger.Log(LogType.Error, NotEnoughBitsError(ShortName, $"{default(short)}"));
            return default;
        }

        short value = Converter.ShortFromBits(Data, ReadBit);
        ReadBit += sizeof(short) * BitsPerByte;
        return value;
    }

    /// <summary>Retrieves a <see cref="ushort"/> from the message.</summary>
    /// <returns>The <see cref="ushort"/> that was retrieved.</returns>
    public ushort GetUShort() {
        if (UnreadBits < sizeof(ushort) * BitsPerByte) {
            RiptideLogger.Log(LogType.Error, NotEnoughBitsError(UShortName, $"{default(ushort)}"));
            return default;
        }

        ushort value = Converter.UShortFromBits(Data, ReadBit);
        ReadBit += sizeof(ushort) * BitsPerByte;
        return value;
    }

    /// <summary>Adds a <see cref="short"/> array to the message.</summary>
    /// <param name="array">The array to add.</param>
    /// <param name="includeLength">Whether or not to include the length of the array in the message.</param>
    /// <returns>The message that the array was added to.</returns>
    public Message AddShorts(short[] array, bool includeLength = true) {
        if (includeLength) {
            AddVarULong((uint)array.Length);
        }

        if (UnwrittenBits < array.Length * sizeof(short) * BitsPerByte) {
            throw new InsufficientCapacityException(this, array.Length, ShortName, sizeof(short) * BitsPerByte);
        }

        for (int i = 0; i < array.Length; i++) {
            Converter.ShortToBits(array[i], Data, WriteBit);
            WriteBit += sizeof(short) * BitsPerByte;
        }

        return this;
    }

    /// <summary>Adds a <see cref="ushort"/> array to the message.</summary>
    /// <param name="array">The array to add.</param>
    /// <param name="includeLength">Whether or not to include the length of the array in the message.</param>
    /// <returns>The message that the array was added to.</returns>
    public Message AddUShorts(ushort[] array, bool includeLength = true) {
        if (includeLength) {
            AddVarULong((uint)array.Length);
        }

        if (UnwrittenBits < array.Length * sizeof(ushort) * BitsPerByte) {
            throw new InsufficientCapacityException(this, array.Length, UShortName, sizeof(ushort) * BitsPerByte);
        }

        for (int i = 0; i < array.Length; i++) {
            Converter.UShortToBits(array[i], Data, WriteBit);
            WriteBit += sizeof(ushort) * BitsPerByte;
        }

        return this;
    }

    /// <summary>Retrieves a <see cref="short"/> array from the message.</summary>
    /// <returns>The array that was retrieved.</returns>
    public short[] GetShorts() {
        return GetShorts((int)GetVarULong());
    }

    /// <summary>Retrieves a <see cref="short"/> array from the message.</summary>
    /// <param name="amount">The amount of shorts to retrieve.</param>
    /// <returns>The array that was retrieved.</returns>
    public short[] GetShorts(int amount) {
        short[] array = new short[amount];
        ReadShorts(amount, array);
        return array;
    }

    /// <summary>Populates a <see cref="short"/> array with shorts retrieved from the message.</summary>
    /// <param name="intoArray">The array to populate.</param>
    /// <param name="startIndex">The position at which to start populating the array.</param>
    public void GetShorts(short[] intoArray, int startIndex = 0) {
        GetShorts((int)GetVarULong(), intoArray, startIndex);
    }

    /// <summary>Populates a <see cref="short"/> array with shorts retrieved from the message.</summary>
    /// <param name="amount">The amount of shorts to retrieve.</param>
    /// <param name="intoArray">The array to populate.</param>
    /// <param name="startIndex">The position at which to start populating the array.</param>
    public void GetShorts(int amount, short[] intoArray, int startIndex = 0) {
        if (startIndex + amount > intoArray.Length) {
            throw new ArgumentException(nameof(amount), ArrayNotLongEnoughError(amount, intoArray.Length, startIndex, ShortName));
        }

        ReadShorts(amount, intoArray, startIndex);
    }

    /// <summary>Retrieves a <see cref="ushort"/> array from the message.</summary>
    /// <returns>The array that was retrieved.</returns>
    public ushort[] GetUShorts() {
        return GetUShorts((int)GetVarULong());
    }

    /// <summary>Retrieves a <see cref="ushort"/> array from the message.</summary>
    /// <param name="amount">The amount of ushorts to retrieve.</param>
    /// <returns>The array that was retrieved.</returns>
    public ushort[] GetUShorts(int amount) {
        ushort[] array = new ushort[amount];
        ReadUShorts(amount, array);
        return array;
    }

    /// <summary>Populates a <see cref="ushort"/> array with ushorts retrieved from the message.</summary>
    /// <param name="intoArray">The array to populate.</param>
    /// <param name="startIndex">The position at which to start populating the array.</param>
    public void GetUShorts(ushort[] intoArray, int startIndex = 0) {
        GetUShorts((int)GetVarULong(), intoArray, startIndex);
    }

    /// <summary>Populates a <see cref="ushort"/> array with ushorts retrieved from the message.</summary>
    /// <param name="amount">The amount of ushorts to retrieve.</param>
    /// <param name="intoArray">The array to populate.</param>
    /// <param name="startIndex">The position at which to start populating the array.</param>
    public void GetUShorts(int amount, ushort[] intoArray, int startIndex = 0) {
        if (startIndex + amount > intoArray.Length) {
            throw new ArgumentException(nameof(amount), ArrayNotLongEnoughError(amount, intoArray.Length, startIndex, UShortName));
        }

        ReadUShorts(amount, intoArray, startIndex);
    }

    /// <summary>Reads a number of shorts from the message and writes them into the given array.</summary>
    /// <param name="amount">The amount of shorts to read.</param>
    /// <param name="intoArray">The array to write the shorts into.</param>
    /// <param name="startIndex">The position at which to start writing into the array.</param>
    private void ReadShorts(int amount, short[] intoArray, int startIndex = 0) {
        if (UnreadBits < amount * sizeof(short) * BitsPerByte) {
            RiptideLogger.Log(LogType.Error, NotEnoughBitsError(amount, ShortName));
            amount = UnreadBits / (sizeof(short) * BitsPerByte);
        }

        for (int i = 0; i < amount; i++) {
            intoArray[startIndex + i] = Converter.ShortFromBits(Data, ReadBit);
            ReadBit += sizeof(short) * BitsPerByte;
        }
    }

    /// <summary>Reads a number of ushorts from the message and writes them into the given array.</summary>
    /// <param name="amount">The amount of ushorts to read.</param>
    /// <param name="intoArray">The array to write the ushorts into.</param>
    /// <param name="startIndex">The position at which to start writing into the array.</param>
    private void ReadUShorts(int amount, ushort[] intoArray, int startIndex = 0) {
        if (UnreadBits < amount * sizeof(ushort) * BitsPerByte) {
            RiptideLogger.Log(LogType.Error, NotEnoughBitsError(amount, UShortName));
            amount = UnreadBits / (sizeof(ushort) * BitsPerByte);
        }

        for (int i = 0; i < amount; i++) {
            intoArray[startIndex + i] = Converter.UShortFromBits(Data, ReadBit);
            ReadBit += sizeof(ushort) * BitsPerByte;
        }
    }
}