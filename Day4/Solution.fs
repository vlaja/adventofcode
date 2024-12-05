// Day 5: https://adventofcode.com/2024/day/5
module Advent_Of_Code.Solutions

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
      (-1, 0)
      (1, 1) // Diagonal Down-Right
      (-1, -1) // Diagonal Up-Left
      (1, -1) // Diagonal Down-Left
      (-1, 1) ] // Up

let hasValidPosition (row, col) rows cols =
    row >= 0 && col >= 0 && row < rows && col < cols

let rec searchFrom (matrix: list<list<char>>) (word: list<char>) row col (dx, dy) wordIndex =
    if wordIndex = List.length word then
        true
    else
        let nextRow, nextCol = row + dx, col + dy

        let validPosition =
            hasValidPosition (nextRow, nextCol) (List.length matrix) (List.head matrix |> List.length)

        if validPosition && matrix[nextRow][nextCol] = List.item wordIndex word then
            searchFrom matrix word nextRow nextCol (dx, dy) (wordIndex + 1)
        else
            false

let directionalMatcher (matrix: list<list<char>>) (word: list<char>) row col =
    directions
    |> List.choose (fun (dx, dy) ->
        if searchFrom matrix word row col (dx, dy) 1 then
            Some(row, col, dx, dy)
        else
            None)

let xMatcher (matrix: list<list<char>>) (word: list<char>) row col =
    let rows = List.length matrix
    let cols = List.head matrix |> List.length

    // Helper functions for character checks
    let isMOrS (row, col) =
        hasValidPosition (row, col) rows cols
        && (matrix[row][col] = List.item 0 word || matrix[row][col] = List.item 2 word)

    let isA (row, col) =
        hasValidPosition (row, col) rows cols && matrix[row][col] = List.item 1 word

    // Validate all positions in the X-shape
    let validTopLeft = isMOrS (row, col - 1)
    let validTopRight = isMOrS (row, col + 1)
    let validMiddle = isA (row + 1, col)
    let validBottomLeft = isMOrS (row + 2, col - 1)
    let validBottomRight = isMOrS (row + 2, col + 1)

    if
        validTopLeft
        && validTopRight
        && validMiddle
        && validBottomLeft
        && validBottomRight
    then
        [ (row + 1, col, 0, 0) ] // Return center position with dummy direction
    else
        []


let searchWord matrix word (matcher: list<list<char>> -> list<char> -> int -> int -> (int * int * int * int) list) =
    let wordChars = word |> Seq.toList
    let rows = List.length matrix
    let cols = List.head matrix |> List.length

    [ for row in 0 .. rows - 1 do
          for col in 0 .. cols - 1 do
              if matrix[row][col] = List.item 0 wordChars then
                  matcher matrix wordChars row col ]
    |> List.concat
    |> List.distinct

// Part one
let directionalCount () =
    searchWord matrix "XMAS" directionalMatcher
    |> List.length
    |> (fun count -> printfn $"Total count: {count}")

// Part two
let xShapeCount () =
    searchWord matrix "MAS" xMatcher
    |> List.length
    |> (fun count -> printfn $"Total count: {count}")

directionalCount ()
xShapeCount ()