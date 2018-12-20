module AdventOfCode2018.Day5Tests

open System
open Xunit
open FsUnit.Xunit

open Day5;

[<Fact>]
let ``Reduce example polymer`` () =
    let input = "dabAcCaCBAcCcaDA" |> List.ofSeq

    let result = react [] input

    let expected = "dabCBAcaDA" |> List.ofSeq

    result |> should equal expected
