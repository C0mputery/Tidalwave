using System.Runtime.CompilerServices;
using Riptide.Utils;

namespace Riptide;

public partial class Message {
    /// <summary>Adds a positive or negative number to the message, using fewer bits for smaller values.</summary>
    /// <inheritdoc cref="AddVarULong(ulong)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Message AddVarLong(long value) {
        return AddVarULong((ulong)Converter.ZigZagEncode(value));
    }

    /// <summary>Adds a positive number to the message, using fewer bits for smaller values.</summary>
    /// <param name="value">The value to add.</param>
    /// <returns>The message that the value was added to.</returns>
    /// <remarks>The value is added in segments of 8 bits, 1 of which is used to indicate whether or not another segment follows. As a result, small values are
    /// added to the message using fewer bits, while large values will require a few more bits than they would if they were added via <see cref="AddByte(byte)"/>,
    /// <see cref="AddUShort(ushort)"/>, <see cref="AddUInt"/>, or <see cref="AddULong(ulong)"/> (or their signed counterparts).</remarks>
    public Message AddVarULong(ulong value) {
        do {
            byte byteValue = (byte)(value & 0b_0111_1111);
            value >>= 7;
            if (value != 0) // There's more to write
            {
                byteValue |= 0b_1000_0000;
            }

            AddByte(byteValue);
        } while (value != 0);

        return this;
    }

    /// <summary>Retrieves a positive or negative number from the message, using fewer bits for smaller values.</summary>
    /// <inheritdoc cref="GetVarULong()"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long GetVarLong() {
        return Converter.ZigZagDecode((long)GetVarULong());
    }

    /// <summary>Retrieves a positive number from the message, using fewer bits for smaller values.</summary>
    /// <returns>The value that was retrieved.</returns>
    /// <remarks>The value is retrieved in segments of 8 bits, 1 of which is used to indicate whether or not another segment follows. As a result, small values are
    /// retrieved from the message using fewer bits, while large values will require a few more bits than they would if they were retrieved via <see cref="GetByte"/>,
    /// <see cref="GetUShort"/>, <see cref="GetUInt"/>, or <see cref="GetULong"/> (or their signed counterparts).</remarks>
    public ulong GetVarULong() {
        ulong byteValue;
        ulong value = 0;
        int shift = 0;

        do {
            byteValue = GetByte();
            value |= (byteValue & 0b_0111_1111) << shift;
            shift += 7;
        } while ((byteValue & 0b_1000_0000) != 0);

        return value;
    }
}