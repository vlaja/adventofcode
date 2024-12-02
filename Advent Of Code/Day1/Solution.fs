// Day 1: https://adventofcode.com/2024/day/1
module Advent_Of_Code.Day1.Solution

open System.IO
open FSharp.Collections

let inputFilePath = Path.Combine(__SOURCE_DIRECTORY__, "Input.txt")

let lines = File.ReadLines(inputFilePath)

let leftList, rightList =
    lines
    |> Seq.map (fun line ->
        let parts = line.Split(' ') |> Array.filter (fun part -> part.Trim() <> "")
        int parts.[0], int parts.[1])
    |> Seq.fold (fun (leftList, rightList) (x, y) -> (leftList @ [ x ], rightList @ [ y ])) ([], [])
    |> fun (leftList, rightList) -> leftList |> List.toArray, rightList |> List.toArray

// Part one
let distances () =
    let sortedLeft = Array.sort leftList
    let sortedRight = Array.sort rightList

    Array.zip sortedLeft sortedRight
    |> Array.map (fun (l, r) -> (max l r) - (min l r))
    |> Array.reduce (fun acc distance -> acc + distance)
    |> fun (distance) -> printfn $"Total distance: {distance}"

// Part two
let similarity () =
    leftList
    |> Array.map (fun num ->
        let count = rightList |> Array.filter ((=) num) |> Array.length
        num, count)
    |> Array.filter (fun (_num, count) -> count > 0)
    |> Array.map (fun (num, count) -> num * count)
    |> Array.reduce (fun acc similarity -> acc + similarity)
    |> fun (distance) -> printfn $"Similarity score: {distance}"

distances ()
similarity ()
