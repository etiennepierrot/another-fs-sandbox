module ClientInterface

    open FSharp.Control.Tasks.V2
    open System
    open System.Net.Http
    open OpenAPITypeProvider
     
    type ClientApi = OpenAPIV3Provider<"/Users/etienne.pierrot/repos/issuing/openapi/src/OpenApiGenerator/output/openapi.yaml">
    
    let executePost payload (client : HttpClient) = 
        let post (content : HttpContent) (client : HttpClient) (uri: string) = 
            task{
                let! response = client.PostAsync(uri, content)
                let! contentResponse = response.Content.ReadAsStringAsync()
                return contentResponse
            }
        let toHttpContent payload = new StringContent(payload, Text.Encoding.UTF8, "application/json")
        payload |> toHttpContent |> post <| client


    let PostAccount payload (client : HttpClient) = task { 
            let! responseContent = executePost payload client "/accounts" 
            return ClientApi.Schemas.``add-account-response``.Parse(responseContent)
        }

    let PostAccountHolder payload (client : HttpClient) = task { 
            let! responseContent = executePost payload client "/account-holders" 
            return ClientApi.Schemas.``add-account-holder-response``.Parse(responseContent)
        }

    let PostCard payload (client : HttpClient) = task {
            let! responseContent = executePost payload client "cards"
            return ClientApi.Schemas.``add-card-response``.Parse(responseContent)
        }