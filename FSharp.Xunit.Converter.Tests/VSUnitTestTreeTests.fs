module FSharp.Xunit.Converter.VSUnitTestTreeTests

open System
open FSharp.Xunit.Converter
open Microsoft.CodeAnalysis.CSharp
open Swensen.Unquote
open Xunit
open FsCheck.Xunit
open FsCheck

[<Fact>]
let ``Convert returns None when given empty string`` () =
    let emptyString = String.Empty
    let emptySyntaxTree = RoslynParser.parse emptyString
    let csharpSyntaxNode = emptySyntaxTree.GetRoot ()

    let result = VSUnitTestTree.convert csharpSyntaxNode emptyString

    result =! None

[<Fact>]
let ``Convert returns Some UnitTest when string is not empty`` () =
    let string = """using System;
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
    let syntaxTree = RoslynParser.parse string
    let csharpSyntaxNode = syntaxTree.GetRoot ()
    let result = VSUnitTestTree.convert csharpSyntaxNode string

    test <@ Option.isSome result @>

[<Fact>]
let ``Convert returns Some UnitTest with correct namespaces when string is not empty`` () =
    let string = """using System;
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
    let syntaxTree = RoslynParser.parse string
    let csharpSyntaxNode = syntaxTree.GetRoot ()
    let tree = VSUnitTestTree.convert csharpSyntaxNode string |> Option.get

    List.length tree.Namespaces =! 6