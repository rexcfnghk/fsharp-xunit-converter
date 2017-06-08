namespace FSharp.Xunit.Converter.Tests

open System
open FSharp.Xunit.Converter
open Swensen.Unquote
open Xunit

module VSUnitTestTreeTests =

    [<Fact>]
    let ``Empty string returns empty VSUnitTestTree`` () =
        let emptyString = String.Empty

        let result = VSUnitTestTree.parse emptyString

        result =! VSUnitTestTree.Empty
