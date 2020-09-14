module HttpClient
    open System.Net.Http
    open System

    type LoggingHandler(messageHandler) =
        inherit DelegatingHandler(messageHandler)
        override __.SendAsync(request, cancellationToken) =
            // Put break point here is want to debug HTTP calls
            let body = request.Content.ReadAsStringAsync()|> Async.AwaitTask |> Async.RunSynchronously
            printfn "[%A]: %A %s" request.Method request.RequestUri body
            base.SendAsync(request, cancellationToken)

    let CreateClient accessToken uri = 
        let handler1 = new HttpClientHandler (UseCookies = false)
        let handler2 = new LoggingHandler(handler1)
        let httpClient = new HttpClient(handler2, true, BaseAddress=Uri(uri))
        httpClient.DefaultRequestHeaders.Authorization <- Headers.AuthenticationHeaderValue("Bearer", accessToken)
        httpClient