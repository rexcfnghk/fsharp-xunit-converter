namespace FSharp.Xunit.Converter

type CSharpUnitTest =
    { Namespaces: string list }

type VSUnitTestTree = 
    | UnitTestCompilationUnit of CSharpUnitTest

module VSUnitTestTree =

    open Microsoft.CodeAnalysis.CSharp.Syntax
    open Microsoft.CodeAnalysis.CSharp

    let convert (node: CSharpSyntaxNode) s =
        if s = ""
        then None
        else
            let compilationUnitSyntax = node :?> CompilationUnitSyntax
            let namespaces = 
                compilationUnitSyntax.Usings
                |> Seq.cast<UsingDirectiveSyntax>
                |> Seq.map (fun m -> sprintf "%A" m.Name)
                |> Seq.toList
            let csharpUnitTest = 
                { Namespaces = namespaces }
            Some csharpUnitTest