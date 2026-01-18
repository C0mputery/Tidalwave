namespace Riptide;

public partial class Message {
    /// <summary>Adds <paramref name="message"/>'s unread bits to the message.</summary>
    /// <param name="message">The message whose unread bits to add.</param>
    /// <returns>The message that the bits were added to.</returns>
    /// <remarks>This method does not move <paramref name="message"/>'s internal read position!</remarks>
    public Message AddMessage(Message message) {
        return AddMessage(message, message.UnreadBits, message.ReadBit);
    }

    /// <summary>Adds a range of bits from <paramref name="message"/> to the message.</summary>
    /// <param name="message">The message whose bits to add.</param>
    /// <param name="amount">The number of bits to add.</param>
    /// <param name="startBit">The position in <paramref name="message"/> from which to add the bits.</param>
    /// <returns>The message that the bits were added to.</returns>
    /// <remarks>This method does not move <paramref name="message"/>'s internal read position!</remarks>
    public Message AddMessage(Message message, int amount, int startBit) {
        if (UnwrittenBits < amount) {
            throw new InsufficientCapacityException(this, nameof(Message), amount);
        }

        int sourcePos = startBit / BitsPerSegment;
        int sourceBit = startBit % BitsPerSegment;
        int destPos = WriteBit / BitsPerSegment;
        int destBit = WriteBit % BitsPerSegment;
        int bitOffset = destBit - sourceBit;
        int destSegments = (WriteBit + amount) / BitsPerSegment - destPos + 1;

        if (bitOffset == 0) {
            // Source doesn't need to be shifted, source and dest bits span the same number of segments
            ulong firstSegment = message.Data[sourcePos];
            if (destBit == 0) {
                Data[destPos] = firstSegment;
            }
            else {
                Data[destPos] |= firstSegment & ~((1ul << sourceBit) - 1);
            }

            for (int i = 1; i < destSegments; i++) {
                Data[destPos + i] = message.Data[sourcePos + i];
            }
        }
        else if (bitOffset > 0) {
            // Source needs to be shifted left, dest bits may span more segments than source bits
            ulong firstSegment = message.Data[sourcePos] & ~((1ul << sourceBit) - 1);
            firstSegment <<= bitOffset;
            if (destBit == 0) {
                Data[destPos] = firstSegment;
            }
            else {
                Data[destPos] |= firstSegment;
            }

            for (int i = 1; i < destSegments; i++) {
                Data[destPos + i] = (message.Data[sourcePos + i - 1] >> (BitsPerSegment - bitOffset)) | (message.Data[sourcePos + i] << bitOffset);
            }
        }
        else {
            // Source needs to be shifted right, source bits may span more segments than dest bits
            bitOffset = -bitOffset;
            ulong firstSegment = message.Data[sourcePos] & ~((1ul << sourceBit) - 1);
            firstSegment >>= bitOffset;
            if (destBit == 0) {
                Data[destPos] = firstSegment;
            }
            else {
                Data[destPos] |= firstSegment;
            }

            int sourceSegments = (startBit + amount) / BitsPerSegment - sourcePos + 1;
            for (int i = 1; i < sourceSegments; i++) {
                Data[destPos + i - 1] |= message.Data[sourcePos + i] << (BitsPerSegment - bitOffset);
                Data[destPos + i] = message.Data[sourcePos + i] >> bitOffset;
            }
        }

        WriteBit += amount;
        Data[destPos + destSegments - 1] &= (1ul << (WriteBit % BitsPerSegment)) - 1;
        return this;
    }
}