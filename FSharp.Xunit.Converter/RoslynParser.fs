module FSharp.Xunit.Converter.RoslynParser

open Microsoft.CodeAnalysis.CSharp

let parse (s: string) =
    s
    |> CSharpSyntaxTree.ParseText
    :?> CSharpSyntaxTree

