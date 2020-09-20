open Cloudify
open System
open OAuth
open CardProduct
open FSharp.Control.Tasks.V2
open ClientInterface
open HttpClient
open ClientInterface

let createCardProduct clientId = 
        let (partnerCredential, _) =  "partner"  |> GetBasicCredentials 
        GetAccessToken "partner" partnerCredential |> CreateCardProduct clientId

[<EntryPoint>]
let main argv =
    // let (credentials, clientId) = GetBasicCredentials "client"
    let (credentials, clientId) = (
        "YWNrX3poaHBubHF4bnptdTdhcWVhcnJ1cWxnYXN1OjcwMjY2OGM0ZjM1ZDRkN2ZiMzc2NDE2YzIwNjk3NTEw", 
        "cli_2tb3kpomlinebngsv5yhnrjewi")
   
    printfn "client_id : %s " clientId
    printfn "credentials : %s " credentials
    let accessToken = GetAccessToken "client" credentials
    printfn "accessToken : %s " accessToken

    let client = CreateHttpClient accessToken "http://localhost:5041/"   
    task {
        let addAccount = SampleData.AddAccount
        let! account = PostAccount addAccount client
        let addAccountHolder = SampleData.AddAccountHolder account.Id
        let! accountHolder = PostAccountHolder addAccountHolder client 
        let addCard = SampleData.AddCard account.Id accountHolder.Id
        let! card = PostCard addCard client
        let addPhysicalCard = SampleData.AddPhysicalCard account.Id accountHolder.Id
        let! physicalCard = PostPhysicalCard addPhysicalCard client

        printfn "account_id : %s" account.Id
        printfn "account_holder_id : %s" accountHolder.Id
        printfn "card_id : %s" card.Id
        printfn "physical card_id : %s" physicalCard.Id
    }
    |> Async.AwaitTask
    |> Async.RunSynchronously    

    // client.
    0 // return an integer exit code
