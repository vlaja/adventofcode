// Day 2: https://adventofcode.com/2024/day/3
module Advent_Of_Code.Day3.Solution

open System.IO
open System.Text.RegularExpressions
open FSharp.Collections
open FSharp.Core

let inputFilePath = Path.Combine(__SOURCE_DIRECTORY__, "Input.txt")

let lines = File.ReadLines(inputFilePath)
let multiplicationPattern = @"mul\((\d{1,3}),(\d{1,3})\)"
let stopPattern = @"don't\(\)"
let resumePattern = @"do\(\)"

let multiplyMatches (matches: List<Match>) =
    [ for m in matches ->
          let x = int m.Groups[1].Value
          let y = int m.Groups[2].Value
          (x, y) ]
    |> Seq.map (fun (x, y) -> x * y)

let filterMatchesBetweenDontAndDo (matches: seq<Match>) =
    matches
    |> Seq.fold
        (fun (isSkipping, acc) m ->
            match m.Value with
            | v when Regex.IsMatch(v, stopPattern) -> (true, acc)
            | v when Regex.IsMatch(v, resumePattern) -> (false, acc)
            | v when Regex.IsMatch(v, multiplicationPattern) && not isSkipping -> (isSkipping, m :: acc)
            | _ -> (isSkipping, acc))
        (false, [])
    |> snd

let findValidCommands input =
    Regex.Matches(input, multiplicationPattern) |> Seq.toList |> multiplyMatches

let findValidCommandsWithStopAndResume (input: string) =
    Regex.Matches(input, $"{multiplicationPattern}|{stopPattern}|{resumePattern}")
    |> filterMatchesBetweenDontAndDo
    |> multiplyMatches

let summarise finder =
    lines |> String.concat " " |> finder |> Seq.sum

// Part one
let findMatches () =
    summarise findValidCommands
    |> fun sum -> printfn $"Total multiplication score: {sum}"

// Part two
let findEnabledMatches () =
    summarise findValidCommandsWithStopAndResume
    |> fun sum -> printfn $"Total multiplication score with for enabled matches: {sum}"

findMatches ()
findEnabledMatches ()
