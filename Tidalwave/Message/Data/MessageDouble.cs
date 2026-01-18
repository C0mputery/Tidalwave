using System;
using Riptide.Utils;

namespace Riptide;

public partial class Message {
    /// <summary>Adds a <see cref="double"/> to the message.</summary>
    /// <param name="value">The <see cref="double"/> to add.</param>
    /// <returns>The message that the <see cref="double"/> was added to.</returns>
    public Message AddDouble(double value) {
        if (UnwrittenBits < sizeof(double) * BitsPerByte) {
            throw new InsufficientCapacityException(this, DoubleName, sizeof(double) * BitsPerByte);
        }

        Converter.DoubleToBits(value, Data, WriteBit);
        WriteBit += sizeof(double) * BitsPerByte;
        return this;
    }

    /// <summary>Retrieves a <see cref="double"/> from the message.</summary>
    /// <returns>The <see cref="double"/> that was retrieved.</returns>
    public double GetDouble() {
        if (UnreadBits < sizeof(double) * BitsPerByte) {
            RiptideLogger.Log(LogType.Error, NotEnoughBitsError(DoubleName, $"{default(double)}"));
            return default;
        }

        double value = Converter.DoubleFromBits(Data, ReadBit);
        ReadBit += sizeof(double) * BitsPerByte;
        return value;
    }

    /// <summary>Adds a <see cref="double"/> array to the message.</summary>
    /// <param name="array">The array to add.</param>
    /// <param name="includeLength">Whether or not to include the length of the array in the message.</param>
    /// <returns>The message that the array was added to.</returns>
    public Message AddDoubles(double[] array, bool includeLength = true) {
        if (includeLength) {
            AddVarULong((uint)array.Length);
        }

        if (UnwrittenBits < array.Length * sizeof(double) * BitsPerByte) {
            throw new InsufficientCapacityException(this, array.Length, DoubleName, sizeof(double) * BitsPerByte);
        }

        for (int i = 0; i < array.Length; i++) {
            Converter.DoubleToBits(array[i], Data, WriteBit);
            WriteBit += sizeof(double) * BitsPerByte;
        }

        return this;
    }

    /// <summary>Retrieves a <see cref="double"/> array from the message.</summary>
    /// <returns>The array that was retrieved.</returns>
    public double[] GetDoubles() {
        return GetDoubles((int)GetVarULong());
    }

    /// <summary>Retrieves a <see cref="double"/> array from the message.</summary>
    /// <param name="amount">The amount of doubles to retrieve.</param>
    /// <returns>The array that was retrieved.</returns>
    public double[] GetDoubles(int amount) {
        double[] array = new double[amount];
        ReadDoubles(amount, array);
        return array;
    }

    /// <summary>Populates a <see cref="double"/> array with doubles retrieved from the message.</summary>
    /// <param name="intoArray">The array to populate.</param>
    /// <param name="startIndex">The position at which to start populating the array.</param>
    public void GetDoubles(double[] intoArray, int startIndex = 0) {
        GetDoubles((int)GetVarULong(), intoArray, startIndex);
    }

    /// <summary>Populates a <see cref="double"/> array with doubles retrieved from the message.</summary>
    /// <param name="amount">The amount of doubles to retrieve.</param>
    /// <param name="intoArray">The array to populate.</param>
    /// <param name="startIndex">The position at which to start populating the array.</param>
    public void GetDoubles(int amount, double[] intoArray, int startIndex = 0) {
        if (startIndex + amount > intoArray.Length) {
            throw new ArgumentException(nameof(amount), ArrayNotLongEnoughError(amount, intoArray.Length, startIndex, DoubleName));
        }

        ReadDoubles(amount, intoArray, startIndex);
    }

    /// <summary>Reads a number of doubles from the message and writes them into the given array.</summary>
    /// <param name="amount">The amount of doubles to read.</param>
    /// <param name="intoArray">The array to write the doubles into.</param>
    /// <param name="startIndex">The position at which to start writing into the array.</param>
    private void ReadDoubles(int amount, double[] intoArray, int startIndex = 0) {
        if (UnreadBits < amount * sizeof(double) * BitsPerByte) {
            RiptideLogger.Log(LogType.Error, NotEnoughBitsError(amount, DoubleName));
            amount = UnreadBits / (sizeof(double) * BitsPerByte);
        }

        for (int i = 0; i < amount; i++) {
            intoArray[startIndex + i] = Converter.DoubleFromBits(Data, ReadBit);
            ReadBit += sizeof(double) * BitsPerByte;
        }
    }
}