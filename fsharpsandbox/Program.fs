open System
open FSharp.Data
open Parser




[<EntryPoint>]
let main argv =
    RunSample
    // Average : 353 ms
    // Min : 190 ms
    // Max : 952 ms
    // Max 95p : 491 ms
    
    //    |> Seq.iter( fun row -> printfn $"Line: (%s{(fst row)}, %f{(snd row)} ms)" )
//    
//    for row in msft.Rows do
//        printfn "Line: (%s, %s)" (row.GetColumn "@Timestamp") (row.GetColumn "CorrelationId")
    0 // return an integer exit code
