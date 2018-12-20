module AdventOfCode2018.Day6Tests

open Xunit
open FsUnit.Xunit

open Day6

[<Fact>]
let ``getCoordsAtDistance 1`` () =
    let input = { X = 5; Y = 5 }

    let expected =
        [
            { X = 5; Y = 4 }
            { X = 5; Y = 6 }
            { X = 4; Y = 5 }
            { X = 6; Y = 5 }
        ]
        |> Set.ofList

    let actual = getCoordsAtDistance 1 input

    actual |> should equal expected

[<Fact>]
let ``getCoordsAtDistance 2`` () =
    let input = { X = 5; Y = 5 }

    let expected =
        [
            { X = 5; Y = 3 }
            { X = 6; Y = 4 }
            { X = 7; Y = 5 }
            { X = 6; Y = 6 }
            { X = 5; Y = 7 }
            { X = 4; Y = 6 }
            { X = 3; Y = 5 }
            { X = 4; Y = 4 }
        ]
        |> Set.ofList

    let actual = getCoordsAtDistance 2 input

    actual |> should equal expected