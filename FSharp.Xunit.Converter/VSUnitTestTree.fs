namespace FSharp.Xunit.Converter

type VSUnitTestTree = 
    | UnitTest

module VSUnitTestTree =
    open Microsoft.CodeAnalysis.CSharp

    let parse (syntaxTreeConverter: string -> CSharpSyntaxTree) s =
        None