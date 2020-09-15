open Cloudify
open System
open OAuth
open CardProduct
open FSharp.Control.Tasks.V2
open ClientInterface
open HttpClient

let createCardProduct clientId = 
        let (partnerCredential, _) =  "partner"  |> GetBasicCredentials 
        GetAccessToken "partner" partnerCredential |> CreateCardProduct clientId

[<EntryPoint>]
let main argv =
    //let (credentials, clientId) = GetBasicCredentials "client"
    let (credentials, clientId) = (
        "YWNrX3RrbnFsM2M2ejdkdWhwNTZnaG5jcDVxaWl5OjAwODc3YTM1NmIwZTQyNDNhMDg3ZDk0MWNhMmFlNmMx", 
        "cli_cinthp2libse5ept2xz6p3uxbi")
   
    printfn "client_id : %s " clientId
    printfn "credentials : %s " credentials
    let accessToken = GetAccessToken "client" credentials
    printfn "accessToken : %s " accessToken
    
    

    let client = CreateHttpClient accessToken "http://localhost:5041/"   
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
        printfn ""
     
    }
    |> Async.AwaitTask
    |> Async.RunSynchronously    

    // client.
    0 // return an integer exit code
