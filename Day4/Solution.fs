// Day 4: https://adventofcode.com/2024/day/4
module Advent_Of_Code.Day1.Solution

open System.IO
open FSharp.Collections

let inputFilePath = Path.Combine(__SOURCE_DIRECTORY__, "Input.txt")

let lines = File.ReadLines(inputFilePath)

let matrix =
    lines |> Seq.toList |> List.map (fun line -> line.ToCharArray() |> Array.toList)

let directions =
    [ (0, 1) // Right
      (0, -1) // Left
      (1, 0) // Down
      (-1, 0) // Up
      (1, 1) // Diagonal Down-Right
      (-1, -1) // Diagonal Up-Left
      (1, -1) // Diagonal Down-Left
      (-1, 1) ] // Diagonal Up-Right

let isValid (row, col) rows cols =
    row >= 0 && col >= 0 && row < rows && col < cols

let normalizeWord word =
    if word = List.rev word then
        word
    else
        List.min [ word; List.rev word ]

let rec searchFrom (matrix: list<list<char>>) (word: list<char>) row col (dx, dy) wordIndex =
    if wordIndex = List.length word then
        true
    else
        let nextRow, nextCol = row + dx, col + dy

        let validPosition =
            isValid (nextRow, nextCol) (List.length matrix) (List.head matrix |> List.length)

        if validPosition && matrix[nextRow][nextCol] = List.item wordIndex word then
            searchFrom matrix word nextRow nextCol (dx, dy) (wordIndex + 1)
        else
            false

let searchWord (matrix: list<list<char>>) (word: string) =
    let wordChars = word |> Seq.toList
    let rows = List.length matrix
    let cols = List.head matrix |> List.length

    [ for row in 0 .. rows - 1 do
          for col in 0 .. cols - 1 do
              if matrix[row][col] = List.item 0 wordChars then
                  directions
                  |> List.choose (fun (dx, dy) ->
                      if searchFrom matrix wordChars row col (dx, dy) 1 then
                          Some(normalizeWord wordChars, (row, col, dx, dy))
                      else
                          None) ]
    |> List.concat
    |> List.map snd

// Part one
let wordCount () =
    searchWord matrix "XMAS"
    |> List.length
    |> (fun count -> printfn $"Total count: {count}")

wordCount ()
