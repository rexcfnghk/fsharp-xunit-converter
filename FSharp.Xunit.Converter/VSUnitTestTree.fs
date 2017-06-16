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

            let classDeclarationSyntax = 
                compilationUnitSyntax.Members
                |> Seq.collect (function :? NamespaceDeclarationSyntax as x -> x.Members :> seq<MemberDeclarationSyntax> | _ -> Seq.empty)
                |> Seq.choose (function :? ClassDeclarationSyntax as x -> Some x | _ -> None)
                |> Seq.exactlyOne
            let testClass = 
                TestClassName <| classDeclarationSyntax.Identifier.ToString(), 
                classDeclarationSyntax.AttributeLists
                |> Seq.filter (fun s -> let item : AttributeSyntax = s.Attributes.Item.[0] in item.Name.ToString() = "TestClassAttribute")
                |> Seq.exactlyOne
            let csharpUnitTest = 
                { Namespaces = namespaces
                  TestClass = testClass }
            Some csharpUnitTest