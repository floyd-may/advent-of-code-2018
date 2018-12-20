module AdventOfCode2018.Day6

open System
open Common

let rawInput =
    System.IO.File.ReadAllLines("day6.txt")

type Coord = {
    X: int;
    Y: int;
    }
    with
    static member (*) (left, right) = {
        X = left.X * right.X;
        Y = left.Y * right.Y
    }

    static member (+) (left, right) = {
        X = left.X + right.X;
        Y = left.Y + right.Y
    }

type Extent = {
    MinX: int;
    MinY: int;
    MaxX: int;
    MaxY: int;
}

let tryCoord str =
    match str with
    | Regex "([0-9]+), ([0-9]+)" [ x; y ] ->
        { X = x |> int ; Y = y |> int }
        |> Some
    | _ -> None

let coords =
    rawInput
    |> List.ofSeq
    |> List.map tryCoord
    |> List.filter Option.isSome
    |> List.map Option.get

let extents =
    let xs =
        coords
        |> List.map (fun x -> x.X)
    let ys =
        coords
        |> List.map (fun x -> x.Y)
    {
        MinX = List.min xs
        MaxX = List.max xs
        MinY = List.min ys
        MaxY = List.max ys
    }

let size =
    (extents.MaxX - extents.MinX + 1)
    *
    (extents.MaxY - extents.MinY + 1)

type GridCell =
    | Closest of Coord * int
    | Tie of Coord Set * int
    | Unset

let distance c1 c2 =
    let deltaX =
        c1.X - c2.X
        |> Math.Abs
    let deltaY =
        c1.Y - c2.Y
        |> Math.Abs

    deltaX + deltaY

let getCoordsAtDistance distance start =
    let quadrants = [
        { X = -1; Y = -1 }
        { X = 1; Y = -1 }
        { X = 1; Y = 1 }
        { X = -1; Y = 1 }
    ]
    let projectToQuadrants coord =
        List.map (fun q -> q * coord) quadrants

    List.init (distance + 1) id
    |> List.map (fun x -> { X = x; Y = distance - x })
    |> List.collect projectToQuadrants
    |> List.distinct
    |> List.map (fun c -> c + start)
    |> Set.ofList

let replaceCell grid coord length =
    let existing =
        Map.tryFind coord grid
        |> Option.defaultValue Unset

    let newCell =
        match existing with
        | Unset ->
            Closest (coord, length)
        | Tie (_, len) when len > length ->
            Closest (coord, length)
        | Tie (coords, len) when len = length ->
            let newCoords = Set.add coord coords
            Tie (newCoords, length)
        | Tie _ -> existing
        | Closest (c, len) when len = length ->
            Tie ([c; coord] |> Set.ofList, length)
        | Closest (_, len) when len > length ->
            Closest (coord, length)
        | Closest _ -> existing

    Map.add coord newCell grid
