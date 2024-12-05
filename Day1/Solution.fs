// Day 1: https://adventofcode.com/2024/day/1
module Advent_Of_Code.Solutions

open System.IO
open FSharp.Collections

let inputFilePath = Path.Combine(__SOURCE_DIRECTORY__, "Input.txt")

let lines = File.ReadLines(inputFilePath)

let processLine (line: string) =
    let parts = line.Split(' ') |> Array.filter (fun part -> part.Trim() <> "")
    int parts[0], int parts[1]

let leftList, rightList =
    lines
    |> Seq.map processLine
    |> Seq.fold (fun (leftList, rightList) (x, y) -> (x :: leftList, y :: rightList)) ([], [])
    |> fun (leftList, rightList) -> leftList |> List.toArray, rightList |> List.toArray

// Part one
let distances () =
    let sortedLeft = Array.sort leftList
    let sortedRight = Array.sort rightList

    Array.zip sortedLeft sortedRight
    |> Array.map (fun (l, r) -> (max l r) - (min l r))
    |> Array.sum
    |> fun distance -> printfn $"Total distance: {distance}"

// Part two
let similarity () =
    leftList
    |> Array.map (fun num ->
        let count = rightList |> Array.filter ((=) num) |> Array.length
        num, count)
    |> Array.filter (fun (_num, count) -> count > 0)
    |> Array.map (fun (num, count) -> num * count)
    |> Array.sum
    |> fun distance -> printfn $"Similarity score: {distance}"

distances ()
similarity ()
