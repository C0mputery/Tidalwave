using System;
using System.Text;
using Riptide.Utils;

namespace Riptide;

public partial class Message {
    /// <summary>Adds a <see cref="string"/> to the message.</summary>
    /// <param name="value">The <see cref="string"/> to add.</param>
    /// <returns>The message that the <see cref="string"/> was added to.</returns>
    public Message AddString(string value) {
        AddBytes(Encoding.UTF8.GetBytes(value));
        return this;
    }

    /// <summary>Retrieves a <see cref="string"/> from the message.</summary>
    /// <returns>The <see cref="string"/> that was retrieved.</returns>
    public string GetString() {
        int length = (int)GetVarULong(); // Get the length of the string (in bytes, NOT characters)
        if (UnreadBits < length * BitsPerByte) {
            RiptideLogger.Log(LogType.Error, NotEnoughBitsError(StringName, "shortened string"));
            length = UnreadBits / BitsPerByte;
        }

        string value = Encoding.UTF8.GetString(GetBytes(length), 0, length);
        return value;
    }

    /// <summary>Adds a <see cref="string"/> array to the message.</summary>
    /// <param name="array">The array to add.</param>
    /// <param name="includeLength">Whether or not to include the length of the array in the message.</param>
    /// <returns>The message that the array was added to.</returns>
    public Message AddStrings(string[] array, bool includeLength = true) {
        if (includeLength) {
            AddVarULong((uint)array.Length);
        }

        // It'd be ideal to throw an exception here (instead of in AddString) if the entire array isn't going to fit, but since each string could
        // be (and most likely is) a different length and some characters use more than a single byte, the only way of doing that would be to loop
        // through the whole array here and convert each string to bytes ahead of time, just to get the required byte count. Then if they all fit
        // into the message, they would all be converted again when actually being written into the byte array, which is obviously inefficient.

        for (int i = 0; i < array.Length; i++) {
            AddString(array[i]);
        }

        return this;
    }

    /// <summary>Retrieves a <see cref="string"/> array from the message.</summary>
    /// <returns>The array that was retrieved.</returns>
    public string[] GetStrings() {
        return GetStrings((int)GetVarULong());
    }

    /// <summary>Retrieves a <see cref="string"/> array from the message.</summary>
    /// <param name="amount">The amount of strings to retrieve.</param>
    /// <returns>The array that was retrieved.</returns>
    public string[] GetStrings(int amount) {
        string[] array = new string[amount];
        for (int i = 0; i < array.Length; i++) {
            array[i] = GetString();
        }

        return array;
    }

    /// <summary>Populates a <see cref="string"/> array with strings retrieved from the message.</summary>
    /// <param name="intoArray">The array to populate.</param>
    /// <param name="startIndex">The position at which to start populating the array.</param>
    public void GetStrings(string[] intoArray, int startIndex = 0) {
        GetStrings((int)GetVarULong(), intoArray, startIndex);
    }

    /// <summary>Populates a <see cref="string"/> array with strings retrieved from the message.</summary>
    /// <param name="amount">The amount of strings to retrieve.</param>
    /// <param name="intoArray">The array to populate.</param>
    /// <param name="startIndex">The position at which to start populating the array.</param>
    public void GetStrings(int amount, string[] intoArray, int startIndex = 0) {
        if (startIndex + amount > intoArray.Length) {
            throw new ArgumentException(nameof(amount), ArrayNotLongEnoughError(amount, intoArray.Length, startIndex, StringName));
        }

        for (int i = 0; i < amount; i++) {
            intoArray[startIndex + i] = GetString();
        }
    }
}