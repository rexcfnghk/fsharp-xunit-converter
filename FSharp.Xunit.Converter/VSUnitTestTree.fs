namespace FSharp.Xunit.Converter

type VSUnitTestTree = 
    | Empty

module VSUnitTestTree =
    let parse s =
        Empty