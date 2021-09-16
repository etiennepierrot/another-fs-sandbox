module ClientInterface

    open FSharp.Control.Tasks.V2
    open System
    open System.Net.Http
    open OpenAPITypeProvider
    type ClientApi = OpenAPIV3Provider<"openapi.json">
    
    let executePost (client : HttpClient) (payload : ObjectValue)  = 
        let post (content : HttpContent) (client : HttpClient) (uri: string) = 
            task{
                let! response = client.PostAsync(uri, content)
                let! contentResponse = response.Content.ReadAsStringAsync()
                return contentResponse
            }
        let toHttpContent payload = new StringContent(payload, Text.Encoding.UTF8, "application/json")
        payload.ToJson() |> toHttpContent |> post <| client

   
    let PostAccount (addAccount : ClientApi.Schemas.``add-account-request``) (client : HttpClient) = 
        task {
            let! jsonResult = addAccount |> executePost client <| "/accounts" 
            return ClientApi.Schemas.``add-account-response``.Parse(jsonResult) 
        }

    let PostAccountHolder (addAccountHolder : ClientApi.Schemas.``add-individual-account-holder-request``) (client : HttpClient) = 
        task {
            let! jsonResult = addAccountHolder |> executePost client <| "/account-holders"
            return ClientApi.Schemas.``add-account-holder-response``.Parse(jsonResult) 
        }
     
    let PostVirtualCard (addCard : ClientApi.Schemas.``add-virtual-card-request``) (client : HttpClient) = 
        task {
            let! jsonResult = addCard |> executePost client <| "/cards"
            return ClientApi.Schemas.``add-card-response``.Parse(jsonResult) 
        }
    let PostPhysicalCard (addCard : ClientApi.Schemas.``add-physical-card-request``) (client : HttpClient) = 
        task {
            let! jsonResult = addCard |> executePost client <| "/cards"
            return ClientApi.Schemas.``add-card-response``.Parse(jsonResult) 
        }

    