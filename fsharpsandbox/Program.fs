open System
open FSharp.Data




[<EntryPoint>]
let main argv =
    let msft = CsvFile.Load("/Users/etienne.pierrot//Downloads/extract-2021-09-14_22-09-98.csv")
    let datediff (arr :DateTime[]) =
       let diff (d1 :DateTime) (d2 :DateTime) = d1.Subtract(d2).TotalMilliseconds |> Math.Abs
       diff arr.[0] arr.[1]
       
    let values = msft.Rows
                    |> Seq.map ( fun row -> ( row.GetColumn "@Timestamp" |> DateTime.Parse, row.GetColumn "CorrelationId"  ))
                    |> Seq.groupBy snd
                    |> Seq.filter(fun row -> row |> snd |> Seq.length = 2)
                    |> Seq.map(fun row -> (fst row,  row |> snd |> Seq.map fst  |> Seq.toArray |> datediff ) ) |> Seq.map snd
    
    values
        |> Seq.sort
        |> Seq.take(int ((99.0 * float (values |> Seq.length) ) / 100.0) )
        |> Seq.last
        |> printfn "%f"
   
    
    // Average : 353 ms
    // Min : 190 ms
    // Max : 952 ms
    // Max 95p : 491 ms
    
    //    |> Seq.iter( fun row -> printfn $"Line: (%s{(fst row)}, %f{(snd row)} ms)" )
//    
//    for row in msft.Rows do
//        printfn "Line: (%s, %s)" (row.GetColumn "@Timestamp") (row.GetColumn "CorrelationId")
    0 // return an integer exit code
