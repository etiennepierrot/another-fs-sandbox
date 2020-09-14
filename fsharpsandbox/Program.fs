open Cloudify
open System
open System.Net.Http
open SwaggerProvider
open OpenAPITypeProvider
open OAuth
open CardProduct
open FSharp.Control.Tasks.V2
open System.Net.Http.Headers;

//let [<Literal>] Schema = "/Users/etienne.pierrot/repos/issuing/openapi/src/OpenApiGenerator/output/openapi.yaml"
type ClientApi = OpenAPIV3Provider<"/Users/etienne.pierrot/repos/issuing/openapi/src/OpenApiGenerator/output/openapi.yaml">

type LoggingHandler(messageHandler) =
    inherit DelegatingHandler(messageHandler)
    override __.SendAsync(request, cancellationToken) =
        // Put break point here is want to debug HTTP calls
        let body = request.Content.ReadAsStringAsync()|> Async.AwaitTask |> Async.RunSynchronously
        printfn "[%A]: %A %s" request.Method request.RequestUri body
        base.SendAsync(request, cancellationToken)

[<EntryPoint>]
let main argv =
    
    let createClient accessToken= 
        let handler1 = new HttpClientHandler (UseCookies = false)
        let baseUri = Uri("http://localhost:5041/")
        let handler2 = new LoggingHandler(handler1)
        use httpClient = new HttpClient(handler2, true, BaseAddress=baseUri)
        httpClient.DefaultRequestHeaders.Authorization <- Headers.AuthenticationHeaderValue("Bearer", accessToken)
        ClientApi.Client(httpClient)

    //let cardProduct = GetBasicCredentials |> GetAccessToken "partner" |> CreateCardProduct
    let accessToken = GetBasicCredentials |> GetAccessToken "client"
    printfn "access_token : %s" accessToken
    
    
    let address = ClientApi.Address(
                        AddressLine1 =  "3 rue des cottages",
                        AddressLine2 =  "3 rue des cottages",
                        City =  "Paris",
                        Zip = "75018",
                        State = "France",
                        Country = "FR")
     
    let client = createClient accessToken               
    task {
        let addAccount = ClientApi.``add-account-request``("EUR", "fake account") 
        let! account = client.PostAccounts(addAccount)
        let addAccountHolder = ClientApi.``add-account-holder-request``(
                                AccountId = account.Id, 
                                Type = "individual", 
                                FirstName = "etienne",
                                LastName = "pierrot",
                                BillingAddress = address
                            ) 
        let! accountHolder = client.PostAccountHolders(addAccountHolder)

        let addCard = ClientApi.``add-card-request``(
                        Type = "virtual",
                        AccountId = account.Id,
                        AccountHolderId = accountHolder.Id
                    )
        let! card = client.PostCards(addCard)

        printfn "account_id : %s" account.Id
        printfn "account_holder_id : %s" accountHolder.Id
        printfn "card_id : %s" card.Id
     
    }
    |> Async.AwaitTask
    |> Async.RunSynchronously    

    // client.
    0 // return an integer exit code
