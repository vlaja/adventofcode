// Day 2: https://adventofcode.com/2024/day/3
module Advent_Of_Code.Day3.Solution

open System.IO
open System.Text.RegularExpressions
open FSharp.Collections

let inputFilePath = Path.Combine(__SOURCE_DIRECTORY__, "Input.txt")

let lines = File.ReadLines(inputFilePath)

let findValidCommands (input: string) =
    let pattern = @"mul\((\d{1,3}),(\d{1,3})\)"
    let matches = Regex.Matches(input, pattern)

    [ for m in matches ->
          let x = int m.Groups.[1].Value
          let y = int m.Groups.[2].Value
          (x, y) ]

// Part one
let findMatches () =
    lines
    |> Seq.collect findValidCommands
    |> Seq.map (fun (x, y) -> x * y)
    |> Seq.sum
    |> fun sum -> printfn $"Total score: {sum}"

findMatches ()
