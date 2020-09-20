module ClientInterface

    open FSharp.Control.Tasks.V2
    open System
    open System.Net.Http
    open OpenAPITypeProvider
    type ClientApi = OpenAPIV3Provider<"/Users/etienne.pierrot/repos/issuing/openapi/src/OpenApiGenerator/output/openapi.json">
    type Request = AddAccount of ClientApi.Schemas.``add-account-request``
                    | AddAccountHolder of ClientApi.Schemas.``add-individual-account-holder-request``
                    | AddCard of ClientApi.Schemas.``add-card-request``
                    | AddPhysicalCard of ClientApi.Schemas.``add-physical-card-request``
    
    let executePost (payload : ObjectValue) (client : HttpClient)  = 
        let post (content : HttpContent) (client : HttpClient) (uri: string) = 
            task{
                let! response = client.PostAsync(uri, content)
                let! contentResponse = response.Content.ReadAsStringAsync()
                return contentResponse
            }
        let toHttpContent payload = new StringContent(payload, Text.Encoding.UTF8, "application/json")
        payload.ToJson() |> toHttpContent |> post <| client

    let executeRequest  (client : HttpClient) (request : Request)  = 
        let execute (payload : ObjectValue) = executePost payload client
        match request with 
        | AddAccount r -> execute  r  "/accounts"
        | AddAccountHolder r -> execute r "/account-holders"
        | AddCard r  -> execute r "/cards"
        | AddPhysicalCard r -> execute r "/cards"

    let PostAccount (addAccount : ClientApi.Schemas.``add-account-request``) (client : HttpClient) = 
        task {
            let! jsonResult = addAccount |> AddAccount |> executeRequest client
            return ClientApi.Schemas.``add-account-response``.Parse(jsonResult) 
        }
    let PostAccountHolder (addAccountHolder : ClientApi.Schemas.``add-individual-account-holder-request``) (client : HttpClient) = 
        task {
            let! jsonResult = addAccountHolder |> AddAccountHolder |> executeRequest client
            return ClientApi.Schemas.``add-account-holder-response``.Parse(jsonResult) 
        }
     
    let PostCard (addCard : ClientApi.Schemas.``add-card-request``) (client : HttpClient) = 
        task {
            let! jsonResult = addCard |> AddCard |> executeRequest client
            return ClientApi.Schemas.``add-card-response``.Parse(jsonResult) 
        }
    let PostPhysicalCard (addCard : ClientApi.Schemas.``add-physical-card-request``) (client : HttpClient) = 
        task {
            let! jsonResult = addCard |> AddPhysicalCard |> executeRequest client
            return ClientApi.Schemas.``add-card-response``.Parse(jsonResult) 
        }

    