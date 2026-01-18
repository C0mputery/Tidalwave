// This file is provided under The MIT License as part of RiptideNetworking.
// Copyright (c) Tom Weiland
// For additional information please see the included LICENSE.md file or view it on GitHub:
// https://github.com/RiptideNetworking/Riptide/blob/main/LICENSE.md

using Riptide.Utils;

namespace Riptide;

/// <summary>Provides functionality for converting data to bytes and vice versa.</summary>
public partial class Message {
    /// <summary>The number of bits in a byte.</summary>
    private const int BitsPerByte = Converter.BitsPerByte;

    /// <summary>The number of bits in each data segment.</summary>
    private const int BitsPerSegment = Converter.BitsPerULong;

    /// <summary> The maximum number of bits this message can hold. </summary>
    public readonly int MaxBitCount;

    /// <summary>The next bit to be read.</summary>
    public int ReadBit { get; private set; }

    /// <summary>The next bit to be written.</summary>
    public int WriteBit { get; private set; }
    
    /// <summary>How many unretrieved bits remain in the message.</summary>
    public int UnreadBits => WriteBit - ReadBit;
    
    /// <summary>How many more bits can be added to the message.</summary>
    public int UnwrittenBits => MaxBitCount - WriteBit;
        
    /// <summary>The message's data.</summary>
    internal readonly ulong[] Data;
}