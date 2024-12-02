module Advent_Of_Code.Day2.Solution

open System.IO
open FSharp.Collections

let inputFilePath = Path.Combine(__SOURCE_DIRECTORY__, "Input.txt")

let lines = File.ReadLines(inputFilePath)

let isSortedAscending (numbers: int[]) =
    numbers |> Array.pairwise |> Array.forall (fun (a, b) -> a <= b)

let isSortedDescending (numbers: int[]) =
    numbers |> Array.pairwise |> Array.forall (fun (a, b) -> a >= b)

let hasValidDifferences (numbers: int[], minOffset: int, maxOffset: int) =
    numbers
    |> Array.pairwise
    |> Array.forall (fun (a, b) ->
        let difference = abs (b - a)
        difference >= minOffset && difference <= maxOffset)

let isSafe numbers =
    let isSorted = isSortedAscending numbers || isSortedDescending numbers
    let hasValidOffsets = hasValidDifferences (numbers, 1, 3)
    isSorted && hasValidOffsets

let levels =
    lines
    |> Seq.map (fun line -> line.Split(' ') |> Array.filter (fun part -> part.Trim() <> "") |> Array.map int)

let countSafeReports () =
    levels
    |> Seq.map (fun numbers -> numbers, isSafe numbers)
    |> Seq.filter (fun (numbers, isSafe) -> isSafe = true)
    |> Seq.toList
    |> List.length
    |> fun (count) -> printfn $"Number of safe levels: {count}"
    
let countSafeReportsWithDampener () =
    levels
    |> Seq.map (fun numbers -> numbers, isSafe numbers)
    |> Seq.filter (fun (numbers, isSafe) -> isSafe = true)
    |> Seq.toList
    |> List.length
    |> fun (count) -> printfn $"Number of safe levels with problem dampener enabled: {count}"

countSafeReports ()
countSafeReportsWithDampener()