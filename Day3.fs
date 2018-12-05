module AdventOfCode2018.Day3

open System.Text.RegularExpressions

type Coord = int * int

type Claim = {
    ID: int;
    TopLeft: Coord;
    Width: int;
    Height: int;
}

let rawInput =
    System.IO.File.ReadAllLines("day3.txt")
    |> List.ofArray

let toCoords claim =
    let top = claim.TopLeft |> snd
    let left = claim.TopLeft |> fst

    let xCoords = Seq.init claim.Width (fun x -> x + left)
    let yCoords = Seq.init claim.Height (fun y -> y + top)

    seq {
        for x in xCoords do
            for y in yCoords do
                yield (x,y)
    }

let regexMatches pattern input =
    let m = Regex.Match(input, pattern)

    List.tail [ for g in m.Groups -> g.Value ]

let claimRegex = @"#([0-9]+) @ ([0-9]+),([0-9]+): ([0-9]+)x([0-9]+)"

// form #123 @ 3,2: 5x4
let parseClaim str =
    let parts =
        regexMatches claimRegex str
        |> List.map int

    match parts with
    | id::left::top::width::height::[] ->
    {
        ID = id;
        TopLeft = (left, top);
        Width = width;
        Height = height;
    }

let claims =
    rawInput
    |> List.map parseClaim

let part1 =
    let allCoords =
        claims
        |> Seq.ofList
        |> Seq.collect toCoords

    let duplicateCoords =
        allCoords
        |> Seq.groupBy id
        |> Seq.where (fun gr -> (snd gr |> Seq.length) > 1)
        |> Seq.map fst

    printfn "Day 3 Part 1: %i" (duplicateCoords |> Seq.length)

let part2 =
    let uniqueClaimCoords =
        claims
        |> Seq.ofList
        |> Seq.collect
            (fun claim ->
                toCoords claim
                |> Seq.map (fun coord -> (claim, coord)))
        |> Seq.groupBy snd
        |> Seq.where (fun gr -> (snd gr |> Seq.length) = 1)
        |> Seq.collect snd

    let coordsByCandidateClaim =
        uniqueClaimCoords
        |> Seq.groupBy fst

    let coveredClaims =
        coordsByCandidateClaim
        |> Seq.map (fun pair -> (fst pair, snd pair, pair |> fst |> toCoords))
        |> Seq.where (fun (_, uncovered, coords) -> (Seq.length uncovered) = (Seq.length coords))
        |> Seq.map (fun (x, _, _) -> x.ID)

    printfn "Day 3 Part 2: %i" (coveredClaims |> Seq.head)
