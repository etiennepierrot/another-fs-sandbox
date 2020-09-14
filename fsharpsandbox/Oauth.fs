module OAuth

    open FSharp.Data
    open FSharp.Data.HttpRequestHeaders
    open FSharp.Data.JsonExtensions

    let identityProviderUrl = "http://localhost:5051/"

    let GetBasicCredentials = 
        let response = Http.RequestString ( 
                            identityProviderUrl + "clients", 
                            headers = [ ContentType HttpContentTypes.Json ],
                            body = TextRequest """{"scopes": ["issuing:client", "issuing:partner"]}""")
                            |> JsonValue.Parse
        response?basic_credentials.AsString()
    let partnerApiUrl = "http://localhost:5041/"
    

    let GetAccessToken scope basicCredentials= 
        let response = Http.RequestString ( 
                            identityProviderUrl + "connect/token", 
                            headers = [ContentType HttpContentTypes.FormValues; Authorization ("Basic " + basicCredentials)],
                            body = FormValues ["grant_type", "client_credentials"; "scope", "issuing:" + scope ])
                            |> JsonValue.Parse
        response?access_token.AsString();

