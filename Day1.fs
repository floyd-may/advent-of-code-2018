module AdventOfCode2018.Day1

open System
open System.IO

let rawInput =
    System.IO.File.ReadAllLines("day1.txt")
    |> Seq.ofArray

let convertInputLine (line:string) =
    line.Trim().Replace("+", "") |> int

let input =
    rawInput
    |> Seq.map convertInputLine
let part1 =
    let result = input |> Seq.sum

    printfn "Day 1 Part 1 Result: %i" result

let infiniteInput =
    Seq.initInfinite (fun x -> input)
    |> Seq.collect id

let frequencies =
    infiniteInput
    |> Seq.scan (+) 0

type FrequencyResult = {
    Current: int;
    Seen: int Set;
    Repeated: bool
}

let frequencyFolder result frequency = {
    Current = frequency;
    Seen = result.Seen
        |> Set.add frequency;
    Repeated = result.Seen |> Set.contains frequency;
}

let part2 =
    let start = {
        Current = 0;
        Seen = Set.singleton 0;
        Repeated = false;
    }

    let debugResult =
        frequencies
        |> Seq.skip 1
        |> Seq.scan frequencyFolder start

    let result =
        debugResult
        |> Seq.skipWhile (fun res -> not res.Repeated)
        |> Seq.map (fun x -> x.Current)

    printfn "Day 1 Part 2 Debug Result: %A" (debugResult |> Seq.take 4)
    printfn "Day 1 Part 2 Result: %i" (result |> Seq.head)
