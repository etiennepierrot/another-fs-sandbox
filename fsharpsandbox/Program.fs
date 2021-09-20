open FSharp.Data
open Parser

[<EntryPoint>]
let main argv =
    //dotnet run "/home/tonton/repos/another-fs-sandbox/fsharpsandbox/extract-2021-09-14_22-09-98.csv
    (Parse "CorrelationId" "date" 
        >> CalculateStat  
        >> DisplayStat) (argv.[0])
    0 // return an integer exit code
