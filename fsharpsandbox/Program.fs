open Cloudify
open System
open OAuth
open CardProduct
open FSharp.Control.Tasks.V2
open ClientInterface
open HttpClient

[<EntryPoint>]
let main argv =
    
    //let cardProduct = GetBasicCredentials |> GetAccessToken "partner" |> CreateCardProduct
    let accessToken = GetBasicCredentials |> GetAccessToken "client"
    printfn "access_token : %s" accessToken
    
    let client = CreateClient accessToken "http://localhost:5041/"              
    task {

        let addAccount = SampleData.AddAccount.ToJson()
        let! account = PostAccount addAccount client
        let addAccountHolder = SampleData.AddAccountHolder(account.Id).ToJson()
        let! accountHolder = PostAccountHolder addAccountHolder client
        let addCard = (SampleData.AddCard account.Id accountHolder.Id).ToJson()
        let! card = PostCard addCard client

        printfn "account_id : %s" account.Id
        printfn "account_holder_id : %s" accountHolder.Id
        printfn "card_id : %s" card.Id
     
    }
    |> Async.AwaitTask
    |> Async.RunSynchronously    

    // client.
    0 // return an integer exit code
