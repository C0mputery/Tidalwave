using System;
using Riptide.Utils;

namespace Riptide;

public partial class Message {
    /// <summary>Adds a <see cref="byte"/> to the message.</summary>
    /// <param name="value">The <see cref="byte"/> to add.</param>
    /// <returns>The message that the <see cref="byte"/> was added to.</returns>
    public Message AddByte(byte value) {
        if (UnwrittenBits < BitsPerByte) {
            throw new InsufficientCapacityException(this, ByteName, BitsPerByte);
        }

        Converter.ByteToBits(value, Data, WriteBit);
        WriteBit += BitsPerByte;
        return this;
    }

    /// <summary>Adds an <see cref="sbyte"/> to the message.</summary>
    /// <param name="value">The <see cref="sbyte"/> to add.</param>
    /// <returns>The message that the <see cref="sbyte"/> was added to.</returns>
    public Message AddSByte(sbyte value) {
        if (UnwrittenBits < BitsPerByte) {
            throw new InsufficientCapacityException(this, SByteName, BitsPerByte);
        }

        Converter.SByteToBits(value, Data, WriteBit);
        WriteBit += BitsPerByte;
        return this;
    }

    /// <summary>Retrieves a <see cref="byte"/> from the message.</summary>
    /// <returns>The <see cref="byte"/> that was retrieved.</returns>
    public byte GetByte() {
        if (UnreadBits < BitsPerByte) {
            RiptideLogger.Log(LogType.Error, NotEnoughBitsError(ByteName, $"{default(byte)}"));
            return default;
        }

        byte value = Converter.ByteFromBits(Data, ReadBit);
        ReadBit += BitsPerByte;
        return value;
    }

    /// <summary>Retrieves an <see cref="sbyte"/> from the message.</summary>
    /// <returns>The <see cref="sbyte"/> that was retrieved.</returns>
    public sbyte GetSByte() {
        if (UnreadBits < BitsPerByte) {
            RiptideLogger.Log(LogType.Error, NotEnoughBitsError(SByteName, $"{default(sbyte)}"));
            return default;
        }

        sbyte value = Converter.SByteFromBits(Data, ReadBit);
        ReadBit += BitsPerByte;
        return value;
    }

    /// <summary>Adds a <see cref="byte"/> array to the message.</summary>
    /// <param name="array">The array to add.</param>
    /// <param name="includeLength">Whether or not to include the length of the array in the message.</param>
    /// <returns>The message that the array was added to.</returns>
    public Message AddBytes(byte[] array, bool includeLength = true) {
        if (includeLength) {
            AddVarULong((uint)array.Length);
        }

        int writeAmount = array.Length * BitsPerByte;
        if (UnwrittenBits < writeAmount) {
            throw new InsufficientCapacityException(this, array.Length, ByteName, BitsPerByte);
        }

        if (WriteBit % BitsPerByte == 0) {
            int bit = WriteBit % BitsPerSegment;
            if (bit + writeAmount > BitsPerSegment) // Range reaches into subsequent segment(s)
            {
                Data[(WriteBit + writeAmount) / BitsPerSegment] = 0;
            }
            else if (bit == 0) // Range doesn't fill the current segment, but begins the segment
            {
                Data[WriteBit / BitsPerSegment] = 0;
            }

            Buffer.BlockCopy(array, 0, Data, WriteBit / BitsPerByte, array.Length);
            WriteBit += writeAmount;
        }
        else {
            for (int i = 0; i < array.Length; i++) {
                Converter.ByteToBits(array[i], Data, WriteBit);
                WriteBit += BitsPerByte;
            }
        }

        return this;
    }

    /// <summary>Adds a <see cref="byte"/> array to the message.</summary>
    /// <param name="array">The array to add.</param>
    /// <param name="startIndex">The position at which to start adding from the array.</param>
    /// <param name="amount">The amount of bytes to add from the startIndex of the array.</param>
    /// <param name="includeLength">Whether or not to include the <paramref name="amount"/> in the message.</param>
    /// <returns>The message that the array was added to.</returns>
    public Message AddBytes(byte[] array, int startIndex, int amount, bool includeLength = true) {
        if (startIndex < 0 || startIndex >= array.Length) {
            throw new ArgumentOutOfRangeException(nameof(startIndex));
        }

        if (startIndex + amount > array.Length) {
            throw new ArgumentException(nameof(amount), $"The source array is not long enough to read {amount} {Helper.CorrectForm(amount, ByteName)} starting at {startIndex}!");
        }

        if (includeLength) {
            AddVarULong((uint)amount);
        }

        int writeAmount = amount * BitsPerByte;
        if (UnwrittenBits < writeAmount) {
            throw new InsufficientCapacityException(this, amount, ByteName, BitsPerByte);
        }

        if (WriteBit % BitsPerByte == 0) {
            int bit = WriteBit % BitsPerSegment;
            if (bit + writeAmount > BitsPerSegment) // Range reaches into subsequent segment(s)
            {
                Data[(WriteBit + writeAmount) / BitsPerSegment] = 0;
            }
            else if (bit == 0) // Range doesn't fill the current segment, but begins the segment
            {
                Data[WriteBit / BitsPerSegment] = 0;
            }

            Buffer.BlockCopy(array, startIndex, Data, WriteBit / BitsPerByte, amount);
            WriteBit += writeAmount;
        }
        else {
            for (int i = startIndex; i < startIndex + amount; i++) {
                Converter.ByteToBits(array[i], Data, WriteBit);
                WriteBit += BitsPerByte;
            }
        }

        return this;
    }

    /// <summary>Adds an <see cref="sbyte"/> array to the message.</summary>
    /// <param name="array">The array to add.</param>
    /// <param name="includeLength">Whether or not to include the length of the array in the message.</param>
    /// <returns>The message that the array was added to.</returns>
    public Message AddSBytes(sbyte[] array, bool includeLength = true) {
        if (includeLength) {
            AddVarULong((uint)array.Length);
        }

        if (UnwrittenBits < array.Length * BitsPerByte) {
            throw new InsufficientCapacityException(this, array.Length, SByteName, BitsPerByte);
        }

        for (int i = 0; i < array.Length; i++) {
            Converter.SByteToBits(array[i], Data, WriteBit);
            WriteBit += BitsPerByte;
        }

        return this;
    }

    /// <summary>Retrieves a <see cref="byte"/> array from the message.</summary>
    /// <returns>The array that was retrieved.</returns>
    public byte[] GetBytes() {
        return GetBytes((int)GetVarULong());
    }

    /// <summary>Retrieves a <see cref="byte"/> array from the message.</summary>
    /// <param name="amount">The amount of bytes to retrieve.</param>
    /// <returns>The array that was retrieved.</returns>
    public byte[] GetBytes(int amount) {
        byte[] array = new byte[amount];
        ReadBytes(amount, array);
        return array;
    }

    /// <summary>Populates a <see cref="byte"/> array with bytes retrieved from the message.</summary>
    /// <param name="intoArray">The array to populate.</param>
    /// <param name="startIndex">The position at which to start populating the array.</param>
    public void GetBytes(byte[] intoArray, int startIndex = 0) {
        GetBytes((int)GetVarULong(), intoArray, startIndex);
    }

    /// <summary>Populates a <see cref="byte"/> array with bytes retrieved from the message.</summary>
    /// <param name="amount">The amount of bytes to retrieve.</param>
    /// <param name="intoArray">The array to populate.</param>
    /// <param name="startIndex">The position at which to start populating the array.</param>
    public void GetBytes(int amount, byte[] intoArray, int startIndex = 0) {
        if (startIndex + amount > intoArray.Length) {
            throw new ArgumentException(nameof(amount), ArrayNotLongEnoughError(amount, intoArray.Length, startIndex, ByteName));
        }

        ReadBytes(amount, intoArray, startIndex);
    }

    /// <summary>Retrieves an <see cref="sbyte"/> array from the message.</summary>
    /// <returns>The array that was retrieved.</returns>
    public sbyte[] GetSBytes() {
        return GetSBytes((int)GetVarULong());
    }

    /// <summary>Retrieves an <see cref="sbyte"/> array from the message.</summary>
    /// <param name="amount">The amount of sbytes to retrieve.</param>
    /// <returns>The array that was retrieved.</returns>
    public sbyte[] GetSBytes(int amount) {
        sbyte[] array = new sbyte[amount];
        ReadSBytes(amount, array);
        return array;
    }

    /// <summary>Populates a <see cref="sbyte"/> array with bytes retrieved from the message.</summary>
    /// <param name="intoArray">The array to populate.</param>
    /// <param name="startIndex">The position at which to start populating <paramref name="intoArray"/>.</param>
    public void GetSBytes(sbyte[] intoArray, int startIndex = 0) {
        GetSBytes((int)GetVarULong(), intoArray, startIndex);
    }

    /// <summary>Populates a <see cref="sbyte"/> array with bytes retrieved from the message.</summary>
    /// <param name="amount">The amount of sbytes to retrieve.</param>
    /// <param name="intoArray">The array to populate.</param>
    /// <param name="startIndex">The position at which to start populating <paramref name="intoArray"/>.</param>
    public void GetSBytes(int amount, sbyte[] intoArray, int startIndex = 0) {
        if (startIndex + amount > intoArray.Length) {
            throw new ArgumentException(nameof(amount), ArrayNotLongEnoughError(amount, intoArray.Length, startIndex, SByteName));
        }

        ReadSBytes(amount, intoArray, startIndex);
    }

    /// <summary>Reads a number of bytes from the message and writes them into the given array.</summary>
    /// <param name="amount">The amount of bytes to read.</param>
    /// <param name="intoArray">The array to write the bytes into.</param>
    /// <param name="startIndex">The position at which to start writing into the array.</param>
    private void ReadBytes(int amount, byte[] intoArray, int startIndex = 0) {
        if (UnreadBits < amount * BitsPerByte) {
            RiptideLogger.Log(LogType.Error, NotEnoughBitsError(amount, ByteName));
            amount = UnreadBits / BitsPerByte;
        }

        if (ReadBit % BitsPerByte == 0) {
            Buffer.BlockCopy(Data, ReadBit / BitsPerByte, intoArray, startIndex, amount);
            ReadBit += amount * BitsPerByte;
        }
        else {
            for (int i = 0; i < amount; i++) {
                intoArray[startIndex + i] = Converter.ByteFromBits(Data, ReadBit);
                ReadBit += BitsPerByte;
            }
        }
    }

    /// <summary>Reads a number of sbytes from the message and writes them into the given array.</summary>
    /// <param name="amount">The amount of sbytes to read.</param>
    /// <param name="intoArray">The array to write the sbytes into.</param>
    /// <param name="startIndex">The position at which to start writing into the array.</param>
    private void ReadSBytes(int amount, sbyte[] intoArray, int startIndex = 0) {
        if (UnreadBits < amount * BitsPerByte) {
            RiptideLogger.Log(LogType.Error, NotEnoughBitsError(amount, SByteName));
            amount = UnreadBits / BitsPerByte;
        }

        for (int i = 0; i < amount; i++) {
            intoArray[startIndex + i] = Converter.SByteFromBits(Data, ReadBit);
            ReadBit += BitsPerByte;
        }
    }
}