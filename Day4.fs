module AdventOfCode2018.Day4

open System

open Common

type GuardID = int

type LogEntry =
    | ShiftStart of GuardID * DateTimeOffset
    | FallAsleep of DateTimeOffset
    | WakeUp of DateTimeOffset

let getTime log =
    match log with
    | FallAsleep stamp
    | WakeUp stamp
    | ShiftStart (_, stamp) -> stamp

let parseStamp stamp =
    let withOffset =
        stamp + " +0:00"
        |> DateTimeOffset.Parse

    withOffset.ToOffset(TimeSpan.Zero)

let shiftStartRegex = @"\[(.*)\] Guard #([0-9]+) begins shift"
let fallsAsleepRegex = @"\[(.*)\] falls asleep"
let wakesUpRegex = @"\[(.*)\] wakes up"
let parseLog input =
    match input with
    | Regex shiftStartRegex [ stamp ; guard ] -> ShiftStart(guard |> int, parseStamp stamp)
    | Regex fallsAsleepRegex [ stamp ] -> FallAsleep(parseStamp stamp)
    | Regex wakesUpRegex [ stamp ] -> WakeUp(parseStamp stamp)
    | _ -> failwithf "whoops, didn't match %A" input

let rawInput =
    System.IO.File.ReadAllLines("day4.txt")
    |> List.ofArray

let orderedLogs =
    rawInput
    |> List.map parseLog
    |> List.sortBy getTime

let rec calculateGuardSleepTime guardID existingMinutes logs =
    match logs with
    | FallAsleep start :: WakeUp sleepEnd :: rest ->
        let delta = sleepEnd.Subtract start
        let minutes = List.init (delta.TotalMinutes |> int) (fun x -> start.Minute + x)
        calculateGuardSleepTime guardID (existingMinutes @ minutes) rest
    | FallAsleep start :: ShiftStart (nextGuardId, sleepEnd) :: rest ->
        let delta = sleepEnd.Subtract start
        let minutes = List.init (delta.TotalMinutes |> int) (fun x -> start.Minute + x)
        let rest = ShiftStart (nextGuardId, sleepEnd) :: rest
        calculateGuardSleepTime guardID (existingMinutes @ minutes) rest
    | _ -> ((guardID, existingMinutes), logs)

let rec calculateSleptMinutes logs =
    match logs with
    | ShiftStart (guardID, _) :: rest ->
        let guardData, rest = calculateGuardSleepTime guardID [] rest
        guardData :: calculateSleptMinutes rest
    | _ -> []

let minutesByGuard =
    let sleptMinutes = calculateSleptMinutes orderedLogs

    sleptMinutes
    |> List.groupBy fst
    |> List.map (fun x -> (fst x, snd x |> List.collect snd))
    |> List.sortByDescending (snd >> List.length)

let frequencies lst =
    lst
    |> List.groupBy id
    |> List.sortByDescending (snd >> List.length)
    |> List.map (fun x -> (fst x), (snd x |> List.length))

let mostFrequent lst =
    frequencies lst
    |> List.head
    |> fst

let mostFrequentMinuteByGuard =
    minutesByGuard
    |> List.map (fun x -> (fst x), (snd x |> frequencies |> List.tryHead))
    |> List.filter (snd >> Option.isSome)
    |> List.map (fun x -> (fst x), (snd x |> Option.get))
    |> List.sortByDescending (snd >> snd)

let part1 =
    let entry = minutesByGuard.Head

    let guardId = fst entry
    let chosenMinute = mostFrequent (snd entry)

    printfn "Day 4 part 1: %A" (guardId * chosenMinute)

let part2 =
    let entry = mostFrequentMinuteByGuard.Head

    let guardId = fst entry
    let chosenMinute = entry |> snd |> fst

    printfn "Day 4 part 2: %A" (guardId * chosenMinute)
