using System;
using Riptide.Utils;

namespace Riptide;

public partial class Message {
    /// <summary>Moves the message's internal write position by the given <paramref name="amount"/> of bits, reserving them so they can be set at a later time.</summary>
    /// <param name="amount">The number of bits to reserve.</param>
    /// <returns>The message instance.</returns>
    public Message ReserveBits(int amount) {
        if (UnwrittenBits < amount) {
            throw new InsufficientCapacityException(this, amount);
        }

        int bit = WriteBit % BitsPerSegment;
        WriteBit += amount;

        // Reset the last segment that the reserved range touches, unless it's also the first one, in which case it may already contain data which we don't want to overwrite
        if (bit + amount >= BitsPerSegment) {
            Data[WriteBit / BitsPerSegment] = 0;
        }

        return this;
    }

    /// <summary>Moves the message's internal read position by the given <paramref name="amount"/> of bits, skipping over them.</summary>
    /// <param name="amount">The number of bits to skip.</param>
    /// <returns>The message instance.</returns>
    public Message SkipBits(int amount) {
        if (UnreadBits < amount) {
            RiptideLogger.Log(LogType.Error, $"Message only contains {UnreadBits} unread {Helper.CorrectForm(UnreadBits, "bit")}, which is not enough to skip {amount}!");
        }

        ReadBit += amount;
        return this;
    }

    /// <summary>Sets up to 64 bits at the specified position in the message.</summary>
    /// <param name="bitfield">The bits to write into the message.</param>
    /// <param name="amount">The number of bits to set.</param>
    /// <param name="startBit">The bit position in the message at which to start writing.</param>
    /// <returns>The message instance.</returns>
    /// <remarks>This method can be used to directly set a range of bits anywhere in the message without moving its internal write position. Data which was previously added to
    /// the message and which falls within the range of bits being set will be <i>overwritten</i>, meaning that improper use of this method will likely corrupt the message!</remarks>
    public Message SetBits(ulong bitfield, int amount, int startBit) {
        if (amount > sizeof(ulong) * BitsPerByte) {
            throw new ArgumentOutOfRangeException(nameof(amount), $"Cannot set more than {sizeof(ulong) * BitsPerByte} bits at a time!");
        }

        Converter.SetBits(bitfield, amount, Data, startBit);
        return this;
    }

    /// <summary>Retrieves up to 8 bits from the specified position in the message.</summary>
    /// <param name="amount">The number of bits to peek.</param>
    /// <param name="startBit">The bit position in the message at which to start peeking.</param>
    /// <param name="bitfield">The bits that were retrieved.</param>
    /// <returns>The message instance.</returns>
    /// <remarks>This method can be used to retrieve a range of bits from anywhere in the message without moving its internal read position.</remarks>
    public Message PeekBits(int amount, int startBit, out byte bitfield) {
        if (amount > BitsPerByte) {
            throw new ArgumentOutOfRangeException(nameof(amount), $"This '{nameof(PeekBits)}' overload cannot be used to peek more than {BitsPerByte} bits at a time!");
        }

        Converter.GetBits(amount, Data, startBit, out bitfield);
        return this;
    }

    /// <summary>Retrieves up to 16 bits from the specified position in the message.</summary>
    /// <inheritdoc cref="PeekBits(int, int, out byte)"/>
    public Message PeekBits(int amount, int startBit, out ushort bitfield) {
        if (amount > sizeof(ushort) * BitsPerByte) {
            throw new ArgumentOutOfRangeException(nameof(amount), $"This '{nameof(PeekBits)}' overload cannot be used to peek more than {sizeof(ushort) * BitsPerByte} bits at a time!");
        }

        Converter.GetBits(amount, Data, startBit, out bitfield);
        return this;
    }

    /// <summary>Retrieves up to 32 bits from the specified position in the message.</summary>
    /// <inheritdoc cref="PeekBits(int, int, out byte)"/>
    public Message PeekBits(int amount, int startBit, out uint bitfield) {
        if (amount > sizeof(uint) * BitsPerByte) {
            throw new ArgumentOutOfRangeException(nameof(amount), $"This '{nameof(PeekBits)}' overload cannot be used to peek more than {sizeof(uint) * BitsPerByte} bits at a time!");
        }

        Converter.GetBits(amount, Data, startBit, out bitfield);
        return this;
    }

    /// <summary>Retrieves up to 64 bits from the specified position in the message.</summary>
    /// <inheritdoc cref="PeekBits(int, int, out byte)"/>
    public Message PeekBits(int amount, int startBit, out ulong bitfield) {
        if (amount > sizeof(ulong) * BitsPerByte) {
            throw new ArgumentOutOfRangeException(nameof(amount), $"This '{nameof(PeekBits)}' overload cannot be used to peek more than {sizeof(ulong) * BitsPerByte} bits at a time!");
        }

        Converter.GetBits(amount, Data, startBit, out bitfield);
        return this;
    }

    /// <summary>Adds up to 8 of the given bits to the message.</summary>
    /// <param name="bitfield">The bits to add.</param>
    /// <param name="amount">The number of bits to add.</param>
    /// <returns>The message that the bits were added to.</returns>
    public Message AddBits(byte bitfield, int amount) {
        if (amount > BitsPerByte) {
            throw new ArgumentOutOfRangeException(nameof(amount), $"This '{nameof(AddBits)}' overload cannot be used to add more than {BitsPerByte} bits at a time!");
        }

        bitfield &= (byte)((1 << amount) - 1); // Discard any bits that are set beyond the ones we're setting
        Converter.ByteToBits(bitfield, Data, WriteBit);
        WriteBit += amount;
        return this;
    }

    /// <summary>Adds up to 16 of the given bits to the message.</summary>
    /// <inheritdoc cref="AddBits(byte, int)"/>
    public Message AddBits(ushort bitfield, int amount) {
        if (amount > sizeof(ushort) * BitsPerByte) {
            throw new ArgumentOutOfRangeException(nameof(amount), $"This '{nameof(AddBits)}' overload cannot be used to add more than {sizeof(ushort) * BitsPerByte} bits at a time!");
        }

        bitfield &= (ushort)((1 << amount) - 1); // Discard any bits that are set beyond the ones we're adding
        Converter.UShortToBits(bitfield, Data, WriteBit);
        WriteBit += amount;
        return this;
    }

    /// <summary>Adds up to 32 of the given bits to the message.</summary>
    /// <inheritdoc cref="AddBits(byte, int)"/>
    public Message AddBits(uint bitfield, int amount) {
        if (amount > sizeof(uint) * BitsPerByte) {
            throw new ArgumentOutOfRangeException(nameof(amount), $"This '{nameof(AddBits)}' overload cannot be used to add more than {sizeof(uint) * BitsPerByte} bits at a time!");
        }

        bitfield &= (1u << (amount - 1) << 1) - 1; // Discard any bits that are set beyond the ones we're adding
        Converter.UIntToBits(bitfield, Data, WriteBit);
        WriteBit += amount;
        return this;
    }

    /// <summary>Adds up to 64 of the given bits to the message.</summary>
    /// <inheritdoc cref="AddBits(byte, int)"/>
    public Message AddBits(ulong bitfield, int amount) {
        if (amount > sizeof(ulong) * BitsPerByte) {
            throw new ArgumentOutOfRangeException(nameof(amount), $"This '{nameof(AddBits)}' overload cannot be used to add more than {sizeof(ulong) * BitsPerByte} bits at a time!");
        }

        bitfield &= (1ul << (amount - 1) << 1) - 1; // Discard any bits that are set beyond the ones we're adding
        Converter.ULongToBits(bitfield, Data, WriteBit);
        WriteBit += amount;
        return this;
    }

    /// <summary>Retrieves the next <paramref name="amount"/> bits (up to 8) from the message.</summary>
    /// <param name="amount">The number of bits to retrieve.</param>
    /// <param name="bitfield">The bits that were retrieved.</param>
    /// <returns>The messages that the bits were retrieved from.</returns>
    public Message GetBits(int amount, out byte bitfield) {
        PeekBits(amount, ReadBit, out bitfield);
        ReadBit += amount;
        return this;
    }

    /// <summary>Retrieves the next <paramref name="amount"/> bits (up to 16) from the message.</summary>
    /// <inheritdoc cref="GetBits(int, out byte)"/>
    public Message GetBits(int amount, out ushort bitfield) {
        PeekBits(amount, ReadBit, out bitfield);
        ReadBit += amount;
        return this;
    }

    /// <summary>Retrieves the next <paramref name="amount"/> bits (up to 32) from the message.</summary>
    /// <inheritdoc cref="GetBits(int, out byte)"/>
    public Message GetBits(int amount, out uint bitfield) {
        PeekBits(amount, ReadBit, out bitfield);
        ReadBit += amount;
        return this;
    }

    /// <summary>Retrieves the next <paramref name="amount"/> bits (up to 64) from the message.</summary>
    /// <inheritdoc cref="GetBits(int, out byte)"/>
    public Message GetBits(int amount, out ulong bitfield) {
        PeekBits(amount, ReadBit, out bitfield);
        ReadBit += amount;
        return this;
    }
}