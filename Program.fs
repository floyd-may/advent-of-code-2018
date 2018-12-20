module AdventOfCode2018.Main

open System
open System.IO
open Xunit.Runners
open System.Reflection
open System.Threading

open Day1;
open Day2;
open Day3;
open Day4;
open Day5;

[<EntryPoint>]
let main argv =
    let assembly = Assembly.GetExecutingAssembly()
    let url = new Uri(assembly.CodeBase)
    let path = Path.GetFullPath url.LocalPath
    use runner = AssemblyRunner.WithoutAppDomain path

    use evt = new ManualResetEvent false

    let indent (str:string) = str.Replace("\n", "\n\t")

    runner.OnExecutionComplete <- fun (_) -> evt.Set() |> ignore
    runner.OnTestFailed <- fun failArgs ->
        printfn
            "Failed: %s.%s: %s"
            failArgs.TypeName
            failArgs.MethodName
            (indent failArgs.ExceptionMessage)

    printfn "Running tests..."
    runner.Start()
    printfn "Tests complete."

    evt.WaitOne(100000) |> ignore

    //Day1.part1 |> ignore
    //Day1.part2 |> ignore

    //Day2.part1 |> ignore
    //Day2.part2 |> ignore

    //Day3.part1 |> ignore
    //Day3.part2 |> ignore

    //Day4.part1 |> ignore
    //Day4.part2 |> ignore

    //Day5.part1 () |> ignore
    //Day5.part2 () |> ignore

    0 // return an integer exit code
