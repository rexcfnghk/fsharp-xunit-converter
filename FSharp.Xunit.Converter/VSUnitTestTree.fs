namespace FSharp.Xunit.Converter

type TestClassName = TestClassName of string

type TestClassAttributeName = TestClassAttributeName of string

type CSharpUnitTest =
    { Namespaces: string list
      TestClass: TestClassName * TestClassAttributeName }

type VSUnitTestTree = 
    | UnitTestCompilationUnit of CSharpUnitTest

module VSUnitTestTree =

    open Microsoft.CodeAnalysis.CSharp.Syntax
    open Microsoft.CodeAnalysis.CSharp

    let convert (node: CSharpSyntaxNode) =
        if node.FullSpan.IsEmpty
        then None
        else
            let compilationUnitSyntax = node :?> CompilationUnitSyntax
            let namespaces = 
                compilationUnitSyntax.Usings
                |> Seq.cast<UsingDirectiveSyntax>
                |> Seq.map (fun m -> sprintf "%A" m.Name)
                |> Seq.toList
            let testClass = TestClassName "", TestClassAttributeName ""
            let csharpUnitTest = 
                { Namespaces = namespaces
                  TestClass = testClass }
            Some csharpUnitTest