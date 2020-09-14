
module ClientInterface

    open FSharp.Control.Tasks.V2
    open System
    open System.Net.Http
    open OpenAPITypeProvider
     
    type ClientApi = OpenAPIV3Provider<"/Users/etienne.pierrot/repos/issuing/openapi/src/OpenApiGenerator/output/openapi.yaml">
    
    let toHttpContent payload = new StringContent(payload, Text.Encoding.UTF8, "application/json")
    
    let executePostQuery  (uri: string)  (client : HttpClient) (content : HttpContent) = 
        task{
            let! response = client.PostAsync(uri, content)
            let! contentResponse = response.Content.ReadAsStringAsync()
            return contentResponse
        }


    let PostAccount payload (client : HttpClient) = 
        task {
            let! responseContent = payload 
                                    |> toHttpContent 
                                    |> executePostQuery "/accounts" client
            return ClientApi.Schemas.``add-account-response``.Parse(responseContent)
        }

    let PostAccountHolder payload (client : HttpClient) = 
        task {

            let! responseContent = payload 
                                    |> toHttpContent 
                                    |> executePostQuery "/account-holders" client
            return ClientApi.Schemas.``add-account-holder-response``.Parse(responseContent)
        }

    let PostCard payload (client : HttpClient) = 
        task {

            let! responseContent = payload 
                                    |> toHttpContent 
                                    |> executePostQuery "/cards" client
            return ClientApi.Schemas.``add-card-response``.Parse(responseContent)
        }