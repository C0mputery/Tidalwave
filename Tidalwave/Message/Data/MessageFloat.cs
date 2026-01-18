using System;
using Riptide.Utils;

namespace Riptide;

public partial class Message {
    /// <summary>Adds a <see cref="float"/> to the message.</summary>
    /// <param name="value">The <see cref="float"/> to add.</param>
    /// <returns>The message that the <see cref="float"/> was added to.</returns>
    public Message AddFloat(float value) {
        if (UnwrittenBits < sizeof(float) * BitsPerByte) {
            throw new InsufficientCapacityException(this, FloatName, sizeof(float) * BitsPerByte);
        }

        Converter.FloatToBits(value, Data, WriteBit);
        WriteBit += sizeof(float) * BitsPerByte;
        return this;
    }

    /// <summary>Retrieves a <see cref="float"/> from the message.</summary>
    /// <returns>The <see cref="float"/> that was retrieved.</returns>
    public float GetFloat() {
        if (UnreadBits < sizeof(float) * BitsPerByte) {
            RiptideLogger.Log(LogType.Error, NotEnoughBitsError(FloatName, $"{default(float)}"));
            return default;
        }

        float value = Converter.FloatFromBits(Data, ReadBit);
        ReadBit += sizeof(float) * BitsPerByte;
        return value;
    }

    /// <summary>Adds a <see cref="float"/> array to the message.</summary>
    /// <param name="array">The array to add.</param>
    /// <param name="includeLength">Whether or not to include the length of the array in the message.</param>
    /// <returns>The message that the array was added to.</returns>
    public Message AddFloats(float[] array, bool includeLength = true) {
        if (includeLength) {
            AddVarULong((uint)array.Length);
        }

        if (UnwrittenBits < array.Length * sizeof(float) * BitsPerByte) {
            throw new InsufficientCapacityException(this, array.Length, FloatName, sizeof(float) * BitsPerByte);
        }

        for (int i = 0; i < array.Length; i++) {
            Converter.FloatToBits(array[i], Data, WriteBit);
            WriteBit += sizeof(float) * BitsPerByte;
        }

        return this;
    }

    /// <summary>Retrieves a <see cref="float"/> array from the message.</summary>
    /// <returns>The array that was retrieved.</returns>
    public float[] GetFloats() {
        return GetFloats((int)GetVarULong());
    }

    /// <summary>Retrieves a <see cref="float"/> array from the message.</summary>
    /// <param name="amount">The amount of floats to retrieve.</param>
    /// <returns>The array that was retrieved.</returns>
    public float[] GetFloats(int amount) {
        float[] array = new float[amount];
        ReadFloats(amount, array);
        return array;
    }

    /// <summary>Populates a <see cref="float"/> array with floats retrieved from the message.</summary>
    /// <param name="intoArray">The array to populate.</param>
    /// <param name="startIndex">The position at which to start populating the array.</param>
    public void GetFloats(float[] intoArray, int startIndex = 0) {
        GetFloats((int)GetVarULong(), intoArray, startIndex);
    }

    /// <summary>Populates a <see cref="float"/> array with floats retrieved from the message.</summary>
    /// <param name="amount">The amount of floats to retrieve.</param>
    /// <param name="intoArray">The array to populate.</param>
    /// <param name="startIndex">The position at which to start populating the array.</param>
    public void GetFloats(int amount, float[] intoArray, int startIndex = 0) {
        if (startIndex + amount > intoArray.Length) {
            throw new ArgumentException(nameof(amount), ArrayNotLongEnoughError(amount, intoArray.Length, startIndex, FloatName));
        }

        ReadFloats(amount, intoArray, startIndex);
    }

    /// <summary>Reads a number of floats from the message and writes them into the given array.</summary>
    /// <param name="amount">The amount of floats to read.</param>
    /// <param name="intoArray">The array to write the floats into.</param>
    /// <param name="startIndex">The position at which to start writing into the array.</param>
    private void ReadFloats(int amount, float[] intoArray, int startIndex = 0) {
        if (UnreadBits < amount * sizeof(float) * BitsPerByte) {
            RiptideLogger.Log(LogType.Error, NotEnoughBitsError(amount, FloatName));
            amount = UnreadBits / (sizeof(float) * BitsPerByte);
        }

        for (int i = 0; i < amount; i++) {
            intoArray[startIndex + i] = Converter.FloatFromBits(Data, ReadBit);
            ReadBit += sizeof(float) * BitsPerByte;
        }
    }
}