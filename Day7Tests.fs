module AdventOfCode2018.Day7Tests

open Xunit
open FsUnit.Xunit

open Day7

[<Fact>]
let ``reduceGraph empty`` () =
    let input = []

    let actual = reduceGraph input

    let expected = ([], false)

    actual |> should equal expected