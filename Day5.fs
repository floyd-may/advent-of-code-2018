module AdventOfCode2018.Day5

open System.Text.RegularExpressions

let rawInput =
    System.IO.File.ReadAllText("day5.txt").Trim()

let inputList =
    rawInput |> List.ofSeq

let letters =
    let a = 'a' |> int
    List.init 26 (fun x -> a + x |> char)

let upper (x:char) =
    let str = new string([|x|])
    str.ToUpper()
    :> char seq
    |> Seq.head

let patterns =
    letters
    |> List.map (fun x -> [(x, upper x); (upper x, x)])
    |> List.collect id
    |> Set.ofList

let rec react (prefix:char list) (input:char list) =
    match input with
    | x :: y :: rest when Set.contains (x, y) patterns ->
        match prefix with
        | [] -> react [] rest
        | last :: prefixRest ->
            let newInput = last :: rest
            react prefixRest newInput
    | x :: rest -> react (x :: prefix) rest
    | [] -> prefix |> List.rev

let part1 () =
    printfn "input: %A" (rawInput.Substring(0, 10))
    let result = react [] inputList |> List.length

    printfn "Day 5 part 1: %A" (result)


let containsLetters (letters: char list) =
    letters
    |> List.exists (fun x -> List.contains x inputList)

let removeAll letters =
    List.filter (fun x -> not (List.contains x letters)) inputList

let part2 () =
    let candidatePolymers =
        letters
        |> List.map (fun x -> [x; upper x])
        |> List.filter (containsLetters)
        |> List.map (fun x -> removeAll x)

    printfn "Candidate polymers length %A" (List.map List.length candidatePolymers)

    let reactedPolymers =
        candidatePolymers
        |> List.map (react [])
        |> List.map List.length

    printfn "Reacted polymers length %A" reactedPolymers

    let result =
        reactedPolymers
        |> List.sortBy id
        |> List.head

    printfn "Day 5 part 2: %A" (result)
