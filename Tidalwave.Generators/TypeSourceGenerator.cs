using System;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Tidalwave.Generators;

[Generator]
public class TypeSourceGenerator : ISourceGenerator {
    public void Initialize(GeneratorInitializationContext context) { }

    public void Execute(GeneratorExecutionContext context) {
        (string TypeName, string Alias)[] types = [
            ("Bool", "bool"),
            ("Byte", "byte"),
            ("SByte", "sbyte"),
            ("Short", "short"),
            ("UShort", "ushort"),
            ("Int", "int"),
            ("UInt", "uint"),
            ("Long", "long"),
            ("ULong", "ulong"),
            ("Float", "float"),
            ("Double", "double")
        ];

        foreach ((string typeName, string alias) in types) {
            string source = CreateTypeSource(typeName, alias);
            context.AddSource($"Tidalwave.Contexts.Types.{typeName}.g.cs", SourceText.From(source, Encoding.UTF8));
        }
    }

    private static string CreateTypeSource(string typeName, string alias) {
        return $$"""
                 using System;
                 using System.Runtime.CompilerServices;
                 
                 namespace Tidalwave {
                     public ref partial struct WriteContext {
                         [MethodImpl(MethodImplOptions.AggressiveInlining)]
                         public void Write{{typeName}}({{alias}} value) {
                             ThrowIfNoSpace(TypeBitSizes.{{typeName}}Size, "{{alias}}");
                             _Write{{typeName}}(value);
                         }
                         
                         [MethodImpl(MethodImplOptions.AggressiveInlining)]
                         public void Write({{alias}} value) {
                             ThrowIfNoSpace(TypeBitSizes.{{typeName}}Size, "{{alias}}");
                             _Write{{typeName}}(value);
                         }
                 
                         [MethodImpl(MethodImplOptions.AggressiveInlining)]
                         public void Write{{typeName}}s(ReadOnlySpan<{{alias}}> values) {
                             int bitsNeeded = values.Length * TypeBitSizes.{{typeName}}Size + TypeBitSizes.IntSize;
                             ThrowIfNoSpace(bitsNeeded, "{{alias}} array");
                             _WriteInt(values.Length);
                             _Write{{typeName}}s(values);
                         }
                         
                         [MethodImpl(MethodImplOptions.AggressiveInlining)]
                         public void Write(Span<{{alias}}> values) {
                             int bitsNeeded = values.Length * TypeBitSizes.{{typeName}}Size + TypeBitSizes.IntSize;
                             ThrowIfNoSpace(bitsNeeded, "{{alias}} array");
                             _WriteInt(values.Length);
                             _Write{{typeName}}s(values);
                         }
                 
                         [MethodImpl(MethodImplOptions.AggressiveInlining)]
                         public void Write{{typeName}}sWithoutLength(ReadOnlySpan<{{alias}}> values) {
                             int bitsNeeded = values.Length * TypeBitSizes.{{typeName}}Size;
                             ThrowIfNoSpace(bitsNeeded, "{{alias}} array without length");
                             _Write{{typeName}}s(values);
                         }
                         
                         [MethodImpl(MethodImplOptions.AggressiveInlining)]
                         public void WriteWithoutLength(Span<{{alias}}> values) {
                             int bitsNeeded = values.Length * TypeBitSizes.{{typeName}}Size + TypeBitSizes.IntSize;
                             ThrowIfNoSpace(bitsNeeded, "{{alias}} array");
                             _WriteInt(values.Length);
                             _Write{{typeName}}s(values);
                         }
                     }
                     
                     
                     public ref partial struct ReadContext {
                         [MethodImpl(MethodImplOptions.AggressiveInlining)]
                         public {{alias}} Read{{typeName}}() {
                             if (!CheckForSpace(TypeBitSizes.{{typeName}}Size)) { return default; }
                             return _Read{{typeName}}();
                         }
                         
                         [MethodImpl(MethodImplOptions.AggressiveInlining)]
                         public bool TryRead{{typeName}}(out {{alias}} value) {
                             if (!CheckForSpace(TypeBitSizes.{{typeName}}Size)) {
                                 value = default;
                                 return false;
                             }
                             value = _Read{{typeName}}();
                             return true;
                         }
                         
                         [MethodImpl(MethodImplOptions.AggressiveInlining)]
                         public {{alias}}[] Read{{typeName}}s() {
                             if (!CheckForSpace(TypeBitSizes.IntSize)) { return Array.Empty<{{alias}}>(); }
                             int peakedLength = _PeakInt();
                             int bitsNeeded = peakedLength * TypeBitSizes.{{typeName}}Size + TypeBitSizes.IntSize;
                             return !CheckForSpace(bitsNeeded) ? Array.Empty<{{alias}}>() : _Read{{typeName}}s(peakedLength);
                         }
                         
                         [MethodImpl(MethodImplOptions.AggressiveInlining)]
                         public bool TryRead{{typeName}}s(out {{alias}}[] values) {
                             if (!CheckForSpace(TypeBitSizes.IntSize)) {
                                 values = Array.Empty<{{alias}}>();
                                 return false;
                             }
                             int peakedLength = _PeakInt();
                             int bitsNeeded = peakedLength * TypeBitSizes.{{typeName}}Size + TypeBitSizes.IntSize;
                             if (!CheckForSpace(bitsNeeded)) {
                                 values = Array.Empty<{{alias}}>();
                                 return false;
                             }
                             values = _Read{{typeName}}s(peakedLength);
                             return true;
                         }
                 
                         [MethodImpl(MethodImplOptions.AggressiveInlining)]
                         public {{alias}}[] Read{{typeName}}s(int count) {
                             int bitsNeeded = count * TypeBitSizes.{{typeName}}Size;
                             return !CheckForSpace(bitsNeeded) ? Array.Empty<{{alias}}>() : _Read{{typeName}}s(count);
                         }
                         
                         [MethodImpl(MethodImplOptions.AggressiveInlining)]
                         public bool TryRead{{typeName}}s(int count, out {{alias}}[] values) {
                             int bitsNeeded = count * TypeBitSizes.{{typeName}}Size;
                             if (!CheckForSpace(bitsNeeded)) {
                                 values = Array.Empty<{{alias}}>();
                                 return false;
                             }
                             values = _Read{{typeName}}s(count);
                             return true;
                         }
                         
                         [MethodImpl(MethodImplOptions.AggressiveInlining)]
                         public {{alias}} Peak{{typeName}}() {
                             if (!CheckForSpace(TypeBitSizes.{{typeName}}Size)) { return default; }
                             return _Peak{{typeName}}();
                         }
                 
                         [MethodImpl(MethodImplOptions.AggressiveInlining)]
                         public bool TryPeak{{typeName}}(out {{alias}} value) {
                             if (!CheckForSpace(TypeBitSizes.{{typeName}}Size)) {
                                 value = default;
                                 return false;
                             }
                 
                             value = _Peak{{typeName}}();
                             return true;
                         }
                     }
                 }
                 
                 """;
    }
}
