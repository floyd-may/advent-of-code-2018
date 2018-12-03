module AdventOfCode2018.Day2

let rawInput =
    System.IO.File.ReadAllLines("day2.txt")
    |> List.ofArray

type BoxID = {
    HasPair: bool;
    HasThree: bool;
}

let toBoxId (str:string) =
    let groups =
        str :> char seq
        |> Seq.groupBy id
        |> Seq.where (fun x -> Seq.length (snd x) > 1)
        |> List.ofSeq

    {
        HasPair = groups
            |> List.exists (fun x -> (Seq.length (snd x)) = 2);
        HasThree = groups
            |> List.exists (fun x -> (Seq.length (snd x)) = 3);
    }

let boxIDs =
    rawInput
    |> List.map toBoxId

let part1 =
    let pairs =
        boxIDs
        |> List.where (fun x -> x.HasPair)
        |> List.length

    let threes =
        boxIDs
        |> List.where (fun x -> x.HasThree)
        |> List.length

    let result = pairs * threes

    printfn "Day 2 Part 1 Result: %i" result

let dropChar idx (str:string) =
    let left = str.[0..idx]
    let right = str.[(idx + 2)..]

    left + right

let getCommonChars dropIdx =
    let subKeys =
        rawInput
        |> List.map (dropChar dropIdx)

    let groups =
        subKeys
        |> List.groupBy id

    let commonChars =
        groups
        |> List.tryFind (fun x -> ((snd x) |> List.length) = 2)

    commonChars
    |> Option.map fst

let part2 =
    let idLength =
        rawInput
        |> List.head
        |> String.length

    let charsToDrop = Seq.init idLength id

    let result =
        charsToDrop
        |> Seq.map getCommonChars
        |> Seq.where Option.isSome
        |> Seq.head
        |> Option.get

    printfn "Day 2 Part 2 Result: %s" result
