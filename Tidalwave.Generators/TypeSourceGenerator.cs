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
            //context.AddSource($"Tidalwave.Contexts.Types.{typeName}.g.cs", SourceText.From(source, Encoding.UTF8));
        }
    }
}
