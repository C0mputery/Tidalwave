using System;
using Riptide.Utils;

namespace Riptide;

public partial class Message {
    /// <summary>Adds a <see cref="bool"/> to the message.</summary>
    /// <param name="value">The <see cref="bool"/> to add.</param>
    /// <returns>The message that the <see cref="bool"/> was added to.</returns>
    public Message AddBool(bool value) {
        if (UnwrittenBits < 1) {
            throw new InsufficientCapacityException(this, BoolName, 1);
        }

        Converter.BoolToBit(value, Data, WriteBit++);
        return this;
    }

    /// <summary>Retrieves a <see cref="bool"/> from the message.</summary>
    /// <returns>The <see cref="bool"/> that was retrieved.</returns>
    public bool GetBool() {
        if (UnreadBits < 1) {
            RiptideLogger.Log(LogType.Error, NotEnoughBitsError(BoolName, $"{default(bool)}"));
            return default;
        }

        return Converter.BoolFromBit(Data, ReadBit++);
    }

    /// <summary>Adds a <see cref="bool"/> array to the message.</summary>
    /// <param name="array">The array to add.</param>
    /// <param name="includeLength">Whether or not to include the length of the array in the message.</param>
    /// <returns>The message that the array was added to.</returns>
    public Message AddBools(bool[] array, bool includeLength = true) {
        if (includeLength) {
            AddVarULong((uint)array.Length);
        }

        if (UnwrittenBits < array.Length) {
            throw new InsufficientCapacityException(this, array.Length, BoolName, 1);
        }

        for (int i = 0; i < array.Length; i++) {
            Converter.BoolToBit(array[i], Data, WriteBit++);
        }

        return this;
    }

    /// <summary>Retrieves a <see cref="bool"/> array from the message.</summary>
    /// <returns>The array that was retrieved.</returns>
    public bool[] GetBools() {
        return GetBools((int)GetVarULong());
    }

    /// <summary>Retrieves a <see cref="bool"/> array from the message.</summary>
    /// <param name="amount">The amount of bools to retrieve.</param>
    /// <returns>The array that was retrieved.</returns>
    public bool[] GetBools(int amount) {
        bool[] array = new bool[amount];
        ReadBools(amount, array);
        return array;
    }

    /// <summary>Populates a <see cref="bool"/> array with bools retrieved from the message.</summary>
    /// <param name="intoArray">The array to populate.</param>
    /// <param name="startIndex">The position at which to start populating the array.</param>
    public void GetBools(bool[] intoArray, int startIndex = 0) {
        GetBools((int)GetVarULong(), intoArray, startIndex);
    }

    /// <summary>Populates a <see cref="bool"/> array with bools retrieved from the message.</summary>
    /// <param name="amount">The amount of bools to retrieve.</param>
    /// <param name="intoArray">The array to populate.</param>
    /// <param name="startIndex">The position at which to start populating the array.</param>
    public void GetBools(int amount, bool[] intoArray, int startIndex = 0) {
        if (startIndex + amount > intoArray.Length) {
            throw new ArgumentException(nameof(amount), ArrayNotLongEnoughError(amount, intoArray.Length, startIndex, BoolName));
        }

        ReadBools(amount, intoArray, startIndex);
    }

    /// <summary>Reads a number of bools from the message and writes them into the given array.</summary>
    /// <param name="amount">The amount of bools to read.</param>
    /// <param name="intoArray">The array to write the bools into.</param>
    /// <param name="startIndex">The position at which to start writing into the array.</param>
    private void ReadBools(int amount, bool[] intoArray, int startIndex = 0) {
        if (UnreadBits < amount) {
            RiptideLogger.Log(LogType.Error, NotEnoughBitsError(amount, BoolName));
            amount = UnreadBits;
        }

        for (int i = 0; i < amount; i++) {
            intoArray[startIndex + i] = Converter.BoolFromBit(Data, ReadBit++);
        }
    }
}