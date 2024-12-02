module Advent_Of_Code.Day2.Solution

open System.IO
open FSharp.Collections

let inputFilePath = Path.Combine(__SOURCE_DIRECTORY__, "Input.txt")

let lines = File.ReadLines(inputFilePath)

let processLine (line: string) =
    line.Split(' ') |> Array.filter (fun part -> part.Trim() <> "") |> Array.map int

let levels = lines |> Seq.map processLine

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

let isSafe (numbers: int[], minOffset: int, maxOffset: int) =
    let isSorted = isSortedAscending numbers || isSortedDescending numbers
    let hasValidOffsets = hasValidDifferences (numbers, minOffset, maxOffset)
    isSorted && hasValidOffsets

let rec generateCombinations (arr: int[]) n =
    if n = 0 then
        [ arr ]
    else
        [ for i in 0 .. Array.length arr - 1 do
              let rest = Array.append arr.[.. i - 1] arr.[i + 1 ..]
              yield! generateCombinations rest (n - 1) ]

let applyDampener (numbers: int[], minOffset: int, maxOffset: int) =
    let numbersToDampen = 1

    [ for i in 1..numbersToDampen do
          yield! generateCombinations numbers i ]
    |> List.exists (fun newArray -> isSafe (newArray, minOffset, maxOffset))

let withDampener isSafe (numbers: int[], minOffset: int, maxOffset: int) =
    match isSafe (numbers, minOffset, maxOffset) with
    | true -> true
    | false -> applyDampener (numbers, minOffset, maxOffset)

let countSafeReportsGeneric safetyCheck =
    levels
    |> Seq.map (fun numbers -> numbers, safetyCheck (numbers, 1, 3))
    |> Seq.filter (fun (numbers, isSafe) -> isSafe = true)
    |> Seq.toList
    |> List.length

let countSafeReports () =
    countSafeReportsGeneric isSafe
    |> fun (count) -> printfn $"Number of safe levels: {count}"

let countSafeReportsWithDampener () =
    countSafeReportsGeneric (withDampener isSafe)
    |> fun (count) -> printfn $"Number of safe levels with problem dampener enabled: {count}"

countSafeReports ()
countSafeReportsWithDampener ()
