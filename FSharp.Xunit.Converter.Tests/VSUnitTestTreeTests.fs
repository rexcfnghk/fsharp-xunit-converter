module FSharp.Xunit.Converter.VSUnitTestTreeTests

open System
open FSharp.Xunit.Converter
open Microsoft.CodeAnalysis.CSharp
open Swensen.Unquote
open Xunit

let [<Literal>] unitTestSource = """using System;
                        using System.Collections.Generic;
                        using System.Linq;
                        using System.Text;
                        using System.Threading.Tasks;
                        using Microsoft.VisualStudio.TestTools.UnitTesting;

                        namespace Test
                        {
                            [TestClass]
                            public class TestClass
                            {
                                [TestMethod]
                                public void Test()
                                {
                                    Assert.IsTrue(true);
                                }
                            }
                        }"""

[<Fact>]
let ``Convert returns None when given a CSharpSyntaxNode with FullSpan equals to zero`` () =
    let emptyString = String.Empty
    let emptySyntaxTree = RoslynParser.parse emptyString

    async {
        let! csharpSyntaxNode = emptySyntaxTree.GetRootAsync () |> Async.AwaitTask
        let result = VSUnitTestTree.convert csharpSyntaxNode
        return result =! None
    } |> Async.StartAsTask

[<Fact>]
let ``Convert returns Some UnitTest when CSharpSyntaxNode contains a unit test`` () =
    let syntaxTree = RoslynParser.parse unitTestSource

    async {
        let! csharpSyntaxNode = syntaxTree.GetRootAsync () |> Async.AwaitTask
        let result = VSUnitTestTree.convert csharpSyntaxNode

        return test <@ Option.isSome result @>
    } |> Async.StartAsTask

[<Fact>]
let ``Convert returns Some UnitTest with correct namespaces when CSharpSyntaxNode contains a unit test`` () =
    let syntaxTree = RoslynParser.parse unitTestSource

    async {
        let! csharpSyntaxNode = syntaxTree.GetRootAsync () |> Async.AwaitTask
        let tree = VSUnitTestTree.convert csharpSyntaxNode |> Option.get

        return List.length tree.Namespaces =! 6
    } |> Async.StartAsTask

[<Fact>]
let ``Convert returns Some UnitTest with correct test class name and TestClass attribute when CSharpSyntaxNode contains a unit test`` () =
    let syntaxTree = RoslynParser.parse unitTestSource

    async {
        let! csharpSyntaxNode = syntaxTree.GetRootAsync () |> Async.AwaitTask

        let tree = VSUnitTestTree.convert csharpSyntaxNode |> Option.get
        let testClass = tree.TestClass
    
        let (TestClassName testClassName, TestClassAttributeName testClassAttributeName) = testClass

        return test <@ testClassName = "Test" && testClassAttributeName = "TestClassAttribute" @>
    } |> Async.StartAsTask