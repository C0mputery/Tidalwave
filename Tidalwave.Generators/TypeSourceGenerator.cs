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

    public string CreateTypeSource(string typeName, string alias) {
        return $$"""
                 using System;
                 using System.Runtime.CompilerServices;
                 
                 namespace Tidalwave {
                     public ref partial struct WriteContext {
                         [MethodImpl(MethodImplOptions.AggressiveInlining)]
                         public void Add{{typeName}}({{alias}} value) {
                             EnsureSpace(TypeBitSizes.{{typeName}}Size, "{{alias}}");
                             Write{{typeName}}(value);
                         }
                         
                         [MethodImpl(MethodImplOptions.AggressiveInlining)]
                         public void Add({{alias}} value) {
                             EnsureSpace(TypeBitSizes.{{typeName}}Size, "{{alias}}");
                             Write{{typeName}}(value);
                         }
                 
                         [MethodImpl(MethodImplOptions.AggressiveInlining)]
                         public void Add{{typeName}}s(ReadOnlySpan<{{alias}}> values) {
                             int bitsNeeded = values.Length * TypeBitSizes.{{typeName}}Size + TypeBitSizes.IntSize;
                             EnsureSpace(bitsNeeded, "{{alias}} array");
                             WriteInt(values.Length);
                             Write{{typeName}}s(values);
                         }
                         
                         [MethodImpl(MethodImplOptions.AggressiveInlining)]
                         public void Add(ReadOnlySpan<{{alias}}> values) {
                             int bitsNeeded = values.Length * TypeBitSizes.{{typeName}}Size + TypeBitSizes.IntSize;
                             EnsureSpace(bitsNeeded, "{{alias}} array");
                             WriteInt(values.Length);
                             Write{{typeName}}s(values);
                         }
                 
                         [MethodImpl(MethodImplOptions.AggressiveInlining)]
                         public void Add{{typeName}}sWithoutLength(ReadOnlySpan<{{alias}}> values) {
                             int bitsNeeded = values.Length * TypeBitSizes.{{typeName}}Size;
                             EnsureSpace(bitsNeeded, "{{alias}} array without length");
                             Write{{typeName}}s(values);
                         }
                         
                         [MethodImpl(MethodImplOptions.AggressiveInlining)]
                         public void AddWithoutLength(ReadOnlySpan<{{alias}}> values) {
                             int bitsNeeded = values.Length * TypeBitSizes.{{typeName}}Size;
                             EnsureSpace(bitsNeeded, "{{alias}} array without length");
                             Write{{typeName}}s(values);
                         }
                     }
                 }
                 """;
    }
}
