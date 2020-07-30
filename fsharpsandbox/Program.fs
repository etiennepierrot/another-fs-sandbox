// Learn more about F# at http://fsharp.org

open System
open System.IO

let directory = "/Users/etienne.pierrot/repos/issuing/api/src/Issuing.ControlAssessment.Api/Control/Domain"

//http://www.fssnip.net/pi/title/Recursively-find-all-files-from-a-sequence-of-directories
let rec allFiles dirs =
    let isSourceFile (filename:string) = filename.EndsWith(".cs")
    if Seq.isEmpty dirs then Seq.empty else
        seq { yield! dirs |> Seq.collect (Directory.EnumerateFiles >> Seq.filter isSourceFile )
              yield! dirs |> Seq.collect Directory.EnumerateDirectories |> allFiles }

let readFile path =
    let isExcludeWord word = 
        [|"public" ;"private"; "protected"; "new"; "return"; "readonly"; "await"; "async"; "get";"set"; "class"; "using"; "namespace"; "var"|] 
        |> Array.contains word
    File.ReadAllLines(path) 
    |> Seq.filter(fun l -> not (l.StartsWith("//"))) 
    |> Seq.collect(fun l -> l.Split([|' ';'\n';'\t';',';'.';'/';'\\';'|';':';';';'{';'}'; '(';')';'='; '<'; '>'|], StringSplitOptions.RemoveEmptyEntries))
    |> Seq.filter( String.IsNullOrWhiteSpace >> not)
    |> Seq.filter(isExcludeWord >> not )
    |> Seq.toArray

type Occurence = {
    Word : string
    Count : int
}

let collectWord path =
    allFiles [|path|] 
        |> Seq.collect readFile
        |> Seq.countBy id
        |> Seq.sortBy (fun (_, c) -> c)
        |> Seq.toList
        |> List.iter( fun (word, count) -> printfn "%s : %d" word count   )



[<EntryPoint>]
let main argv =
    collectWord( "/Users/etienne.pierrot/repos/issuing/api/src/Issuing.ControlAssessment.Api/" )
    0 // return an integer exit code
