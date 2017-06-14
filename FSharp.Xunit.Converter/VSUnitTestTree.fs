namespace FSharp.Xunit.Converter

type VSUnitTestTree = 
    | UnitTest

module VSUnitTestTree =

    open Microsoft.CodeAnalysis.CSharp
    open Microsoft.CodeAnalysis.CSharp.Syntax

    let convert (node: CSharpSyntaxNode) s =
        if s <> ""
        then Some UnitTest
        else None