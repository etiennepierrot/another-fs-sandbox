module HttpClient
    open System.Net.Http
    open System
    
    type internal AsyncCallableHandler(messageHandler) =
        inherit DelegatingHandler(messageHandler)
        member internal x.CallSendAsync(request, cancellationToken) =
            base.SendAsync(request, cancellationToken)

    let loggingHandler =
        { new DelegatingHandler() with
            member x.SendAsync(request, cancellationToken) =
                let wrapped = new AsyncCallableHandler(base.InnerHandler)
                let workflow = async {
                    let! requestContent =
                        request.Content.ReadAsStringAsync()
                        |> Async.AwaitTask
                    printfn "Request : [%A]: %A %s" request.Method request.RequestUri requestContent
                    let! response =
                        wrapped.CallSendAsync(request, cancellationToken)
                        |> Async.AwaitTask
                    let! responseContent =
                        response.Content.ReadAsStringAsync()
                        |> Async.AwaitTask
                    printfn "Response : [%A]: %A %s" request.Method request.RequestUri responseContent
                    return response
                }
                Async.StartAsTask(workflow, cancellationToken = cancellationToken)
        }

    let CreateHttpClient accessToken uri = 
        let handler2 = loggingHandler
        handler2.InnerHandler <- new HttpClientHandler (UseCookies = false)
        let httpClient = new HttpClient(handler2, true, BaseAddress=Uri(uri))
        httpClient.DefaultRequestHeaders.Authorization <- Headers.AuthenticationHeaderValue("Bearer", accessToken)
        httpClient