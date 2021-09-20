module Parser
    open System
    open FSharp.Data

    type DataTrace = { 
        CorrelationId   : string;
        Timestamp       : DateTime
    }

    type Stat = { 
        SizeSample  : int;
        MinValue    : float;
        ``95p``     : float;
        ``99p``     : float;
        MaxValue    : float;
        Average     : float;
        Median      : float;
    }

    let Parse (correlationIdField :string) (dateField :string) (path :string) =
        CsvFile.Load(path).Rows 
        |> Seq.map (fun row -> {
                    CorrelationId = row.GetColumn correlationIdField;
                    Timestamp =  row.GetColumn dateField |> DateTime.Parse;
                    })

    let CalculateStat (traces  :seq<DataTrace>) = 
        let percentile (percentile :float) (values :seq<float>) = 
            let percent totalSample = int ((percentile * float (totalSample) ) / 100.0)
            values
            |> Seq.sort
            |> Seq.take( (values |> Seq.length) |> percent )
            |> Seq.last

        let ``p95`` = percentile 95.0
        let ``p99`` = percentile 99.0
        let median  = percentile 50.0

        let toStat (values :seq<float>) = 
         {
            SizeSample  = values |> Seq.length;
            MinValue    = values |> Seq.min;
            ``95p``     = values |> ``p95``;
            ``99p``     = values |> ``p99``;
            MaxValue    = values |> Seq.max;
            Average     = values |> Seq.average;
            Median      = values |> median
        } 

        let extractLatency (_, datatraces) = 
            let latency  (d1 :DateTime, d2 :DateTime) = d1.Subtract(d2).TotalMilliseconds |> Math.Abs
            let getExtremeValues (datatraces) = (datatraces |> Seq.min, datatraces |> Seq.max)
            datatraces 
            |> Seq.map( fun dt -> dt.Timestamp) 
            |> getExtremeValues
            |> latency

        traces
        |> Seq.groupBy (fun x -> x.CorrelationId)
        |> Seq.filter(fun (_, traces) -> traces |> Seq.length = 2)
        |> Seq.map extractLatency 
        |> toStat

    let DisplayStat (stat :Stat) = 
        printfn "size sample    : %d items" stat.SizeSample
        printfn "min            : %f ms" stat.MinValue
        printfn "95p            : %f ms" stat.``95p``
        printfn "99p            : %f ms" stat.``99p``
        printfn "max            : %f ms" stat.MaxValue
        printfn "avg            : %f ms" stat.Average
        printfn "median         : %f ms" stat.Median 
    