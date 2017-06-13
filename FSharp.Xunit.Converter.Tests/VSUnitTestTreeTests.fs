module FSharp.Xunit.Converter.VSUnitTestTreeTests

open System
open FSharp.Xunit.Converter
open Swensen.Unquote
open Xunit

[<Fact>]
let ``Empty string returns None`` () =
    let emptyString = String.Empty

    let result = VSUnitTestTree.parse RoslynParser.parse emptyString

    result =! None

[<Fact>]
let ``Unit test can be parsed to Some UnitTest`` () =
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
    let result = VSUnitTestTree.parse RoslynParser.parse string

    result =! Some UnitTest
