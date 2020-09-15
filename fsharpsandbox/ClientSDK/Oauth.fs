module OAuth

    open FSharp.Data
    open FSharp.Data.HttpRequestHeaders
    open FSharp.Data.JsonExtensions

    let identityProviderUrl = "http://localhost:5051/"

    let GetBasicCredentials scope = 
        let body scope  =   scope |>  sprintf """{"scopes": ["issuing:%s"]}""" |> TextRequest
        let response = Http.RequestString ( 
                            identityProviderUrl + "clients", 
                            headers = [ ContentType HttpContentTypes.Json ],
                            body = body scope)
                            |> JsonValue.Parse
        (response?basic_credentials.AsString(), response?claims.["cko_client_id"].AsString())
    
    
    let partnerApiUrl = "http://localhost:5041/"
    

    let GetAccessToken scope basicCredentials= 
        printfn "scope : %s" scope
        printfn "basicCredentials : %s" basicCredentials
        let response = Http.RequestString ( 
                                        identityProviderUrl + "connect/token", 
                                        headers = [ContentType HttpContentTypes.FormValues; Authorization ("Basic " + basicCredentials)],
                                        body = FormValues ["grant_type", "client_credentials"; "scope", "issuing:" + scope ])
                                        |> JsonValue.Parse
        response?access_token.AsString()
