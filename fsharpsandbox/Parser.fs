module Parser
    open System
    open FSharp.Data

    let RunSample = 
        let msft = CsvFile.Load("/home/tonton/repos/another-fs-sandbox/fsharpsandbox/extract-2021-09-14_22-09-98.csv")
        let datediff (arr :DateTime[]) =
           let diff (d1 :DateTime) (d2 :DateTime) = d1.Subtract(d2).TotalMilliseconds |> Math.Abs
           diff arr.[0] arr.[1]

        let percentile (percentile :float) (values :seq<float>) = 
            values
            |> Seq.sort
            |> Seq.take(int ((percentile * float (values |> Seq.length) ) / 100.0) )
            |> Seq.last
           
        let values = 
            let parseRow (row :CsvRow) = ( row.GetColumn "@Timestamp" |> DateTime.Parse, row.GetColumn "CorrelationId")
            msft.Rows
            |> Seq.map parseRow
            |> Seq.groupBy snd
            |> Seq.filter(fun row -> row |> snd |> Seq.length = 2)
            |> Seq.map(fun row -> row |> snd |> Seq.map fst  |> Seq.toArray |> datediff) 
        

        printfn "size sample    : %d items" (values |> Seq.length )
        printfn "min            : %f ms" ( values |> Seq.min )
        printfn "95p            : %f ms" ( values |> percentile 95.0)
        printfn "99p            : %f ms" ( values |> percentile 99.0)
        printfn "max            : %f ms" ( values |> Seq.max )
        printfn "avg            : %f ms" ( values |> Seq.average )
        printfn "median         : %f ms" ( values |> percentile 50.0 )


    