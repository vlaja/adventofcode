// Day 5: https://adventofcode.com/2024/day/5
module Advent_Of_Code.Solutions

open System.IO
open FSharp.Collections

let inputFilePath = Path.Combine(__SOURCE_DIRECTORY__, "Input.txt")

let lines = File.ReadLines(inputFilePath)

let middleIndex (lst: int list) =
    if List.isEmpty lst then None else Some(lst.Length / 2)

let orderingRules, orders =
    lines
    |> Seq.fold
        (fun (rules, orderLines) line ->
            match line.Trim() with
            | "" -> (rules, orderLines)
            | _ when line.Contains("|") ->
                let parts = line.Split('|')
                ((int parts.[0], int parts.[1]) :: rules, orderLines)
            | _ when line.Contains(",") -> (rules, (line.Split(',') |> Array.map int |> Array.toList) :: orderLines)
            | _ -> (rules, orderLines))
        ([], [])
    |> fun (rules, orderLines) -> (List.rev rules, List.rev orderLines)

let respectsRules (rules: (int * int) list) (order: int list) =
    rules
    |> List.forall (fun (a, b) ->
        let indexA = order |> List.tryFindIndex ((=) a)
        let indexB = order |> List.tryFindIndex ((=) b)

        match indexA, indexB with
        | Some iA, Some iB -> iA < iB
        | _, _ -> true)

let filterValidOrders (rules: (int * int) list) (orders: int list list) =
    orders |> List.filter (respectsRules rules)

let orderPages () =
    filterValidOrders orderingRules orders
    |> List.choose (fun order ->
        match middleIndex order with
        | Some idx -> Some order.[idx] // Get the value at the middle index
        | None -> None)
    |> List.sum
    |> fun sum -> printf $"Total sum of middle indexes is: {sum}"

orderPages ()
