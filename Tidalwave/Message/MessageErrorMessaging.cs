using Riptide.Utils;

namespace Riptide;

public partial class Message {
    /// <summary>The name of a <see cref="byte"/> value.</summary>
    private const string ByteName = "byte";

    /// <summary>The name of a <see cref="sbyte"/> value.</summary>
    private const string SByteName = "sbyte";

    /// <summary>The name of a <see cref="bool"/> value.</summary>
    private const string BoolName = "bool";

    /// <summary>The name of a <see cref="short"/> value.</summary>
    private const string ShortName = "short";

    /// <summary>The name of a <see cref="ushort"/> value.</summary>
    private const string UShortName = "ushort";

    /// <summary>The name of an <see cref="int"/> value.</summary>
    private const string IntName = "int";

    /// <summary>The name of a <see cref="uint"/> value.</summary>
    private const string UIntName = "uint";

    /// <summary>The name of a <see cref="long"/> value.</summary>
    private const string LongName = "long";

    /// <summary>The name of a <see cref="ulong"/> value.</summary>
    private const string ULongName = "ulong";

    /// <summary>The name of a <see cref="float"/> value.</summary>
    private const string FloatName = "float";

    /// <summary>The name of a <see cref="double"/> value.</summary>
    private const string DoubleName = "double";

    /// <summary>The name of a <see cref="string"/> value.</summary>
    private const string StringName = "string";

    /// <summary>The name of an array length value.</summary>
    private const string ArrayLengthName = "array length";

    /// <summary>Constructs an error message for when a message contains insufficient unread bits to retrieve a certain value.</summary>
    /// <param name="valueName">The name of the value type for which the retrieval attempt failed.</param>
    /// <param name="defaultReturn">Text describing the value which will be returned.</param>
    /// <returns>The error message.</returns>
    private string NotEnoughBitsError(string valueName, string defaultReturn) {
        return $"Message only contains {UnreadBits} unread {Helper.CorrectForm(UnreadBits, "bit")}, which is not enough to retrieve a value of type '{valueName}'! Returning {defaultReturn}.";
    }

    /// <summary>Constructs an error message for when a message contains insufficient unread bits to retrieve an array of values.</summary>
    /// <param name="arrayLength">The expected length of the array.</param>
    /// <param name="valueName">The name of the value type for which the retrieval attempt failed.</param>
    /// <returns>The error message.</returns>
    private string NotEnoughBitsError(int arrayLength, string valueName) {
        return $"Message only contains {UnreadBits} unread {Helper.CorrectForm(UnreadBits, "bit")}, which is not enough to retrieve {arrayLength} {Helper.CorrectForm(arrayLength, valueName)}! Returned array will contain default elements.";
    }

    /// <summary>Constructs an error message for when a number of retrieved values do not fit inside the bounds of the provided array.</summary>
    /// <param name="amount">The number of values being retrieved.</param>
    /// <param name="arrayLength">The length of the provided array.</param>
    /// <param name="startIndex">The position in the array at which to begin writing values.</param>
    /// <param name="valueName">The name of the value type which is being retrieved.</param>
    /// <param name="pluralValueName">The name of the value type in plural form. If left empty, this will be set to <paramref name="valueName"/> with an <c>s</c> appended to it.</param>
    /// <returns>The error message.</returns>
    private string ArrayNotLongEnoughError(int amount, int arrayLength, int startIndex, string valueName, string pluralValueName = "") {
        if (string.IsNullOrEmpty(pluralValueName)) {
            pluralValueName = $"{valueName}s";
        }

        return $"The amount of {pluralValueName} to retrieve ({amount}) is greater than the number of elements from the start index ({startIndex}) to the end of the given array (length: {arrayLength})!";
    }
}