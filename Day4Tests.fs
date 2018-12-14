module AdventOfCode2018.Day4Tests

open System
open Xunit
open FsUnit.Xunit

open Day4;

let epoch = new DateTimeOffset(1518, 11, 1, 0, 0, 0, 0, TimeSpan.Zero)

[<Fact>]
let ``Parse start shift`` () =
    let input = "[1518-11-01 00:00] Guard #10 begins shift"

    let result = parseLog input

    let expected = ShiftStart(10, epoch)

    result |> should equal expected

[<Fact>]
let ``Parse fall asleep`` () =
    let input = "[1518-11-01 00:00] falls asleep"

    let result = parseLog input

    let expected = FallAsleep(epoch)

    result |> should equal expected

[<Fact>]
let ``Parse wake up`` () =
    let input = "[1518-11-01 00:00] wakes up"

    let result = parseLog input

    let expected = WakeUp(epoch)

    result |> should equal expected

[<Fact>]
let ``calculateSleptMinutes with no sleep wake cycles``() =
    let input = [
        ShiftStart(22, epoch);
    ]

    let result = calculateSleptMinutes input

    let expected = [(22, [0].Tail)]

    result |> should equal expected

[<Fact>]
let ``calculateSleptMinutes with single sleep wake cycle``() =
    let input = [
        ShiftStart(22, epoch);
        FallAsleep(epoch.AddMinutes(5.0));
        WakeUp(epoch.AddMinutes(10.0));
    ]

    let result = calculateSleptMinutes input

    let expected = [(22, [5;6;7;8;9;])]

    result |> should equal expected

[<Fact>]
let ``calculateSleptMinutes with multiple sleep wake cycles``() =
    let input = [
        ShiftStart(22, epoch);
        FallAsleep(epoch.AddMinutes(5.0));
        WakeUp(epoch.AddMinutes(10.0));
        FallAsleep(epoch.AddMinutes(25.0));
        WakeUp(epoch.AddMinutes(30.0));
    ]

    let result = calculateSleptMinutes input

    let expected = [(22, [5;6;7;8;9;25;26;27;28;29])]

    result |> should equal expected

[<Fact>]
let ``calculateSleptMinutes with multiple guards``() =
    let input = [
        ShiftStart(22, epoch);
        FallAsleep(epoch.AddMinutes(5.0));
        WakeUp(epoch.AddMinutes(10.0));
        ShiftStart(23, epoch.AddMinutes(20.0));
        FallAsleep(epoch.AddMinutes(25.0));
        WakeUp(epoch.AddMinutes(30.0));
    ]

    let result = calculateSleptMinutes input

    let expected = [
        (22, [5;6;7;8;9;]);
        (23, [25;26;27;28;29])
        ]

    result |> should equal expected

[<Fact>]
let ``calculateSleptMinutes woken by next guard``() =
    let input = [
        ShiftStart(22, epoch);
        FallAsleep(epoch.AddMinutes(5.0));
        ShiftStart(23, epoch.AddMinutes(10.0));
    ]

    let result = calculateSleptMinutes input

    let expected = [
        (22, [5;6;7;8;9;]);
        (23, [])
        ]

    result |> should equal expected