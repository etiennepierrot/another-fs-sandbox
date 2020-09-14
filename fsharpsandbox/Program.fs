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
        let httpClient = new HttpClient(handler2, true, BaseAddress=baseUri)
        httpClient.DefaultRequestHeaders.Authorization <- Headers.AuthenticationHeaderValue("Bearer", accessToken)
        httpClient

    //let cardProduct = GetBasicCredentials |> GetAccessToken "partner" |> CreateCardProduct
    let accessToken = GetBasicCredentials |> GetAccessToken "client"
    printfn "access_token : %s" accessToken
    
    
    let address = ClientApi.Schemas.Address(
                        address_line1 =  "3 rue des cottages",
                        address_line2 =  Some "3 rue des cottages",
                        city =  "Paris",
                        zip = "75018",
                        state = Some "France",
                        country = "FR")
     
    let client = createClient accessToken               
    task {

        let addAccount = ClientApi.Schemas.``add-account-request``("EUR",  Some "fake account").ToJson()
        let contentAddAccount = new StringContent(addAccount, Text.Encoding.UTF8, "application/json")
        let! response = client.PostAsync( "/accounts", contentAddAccount)
        let! content = response.Content.ReadAsStringAsync()
        let account = ClientApi.Schemas.``add-account-response``.Parse(content)

        let addAccountHolder = ClientApi.Schemas.``add-individual-account-holder-request``(
                                    account_id = Some account.Id,
                                    ``type`` = "individual",
                                    first_name = Some "etienne",
                                    last_name = Some "pierrot",
                                    billing_address = address
                                ).ToJson()

        let contentAddAccountHolder = new StringContent(addAccountHolder, Text.Encoding.UTF8, "application/json")
        let! response = client.PostAsync( "/account-holders", contentAddAccountHolder)
        let! content = response.Content.ReadAsStringAsync()
        let accountHolder = ClientApi.Schemas.``add-account-holder-response``.Parse(content)

       

        printfn "account_id : %s" account.Id
        printfn "account_holder_id : %s" accountHolder.Id
     
    }
    |> Async.AwaitTask
    |> Async.RunSynchronously    

    // client.
    0 // return an integer exit code
