module CardProduct
    
    open FSharp.Data
    open FSharp.Data.HttpRequestHeaders

    let partnerApiUrl = "http://localhost:5041/"
    
    let addCardProduct clientId = sprintf """ 
                                    {
                                        "client_id": "%s",
                                        "has_pci_clearance": "true",
                                        "maximum_card_lifetime": {
                                            "units" : "Years",
                                            "value": 3
                                            },
                                        "allowed_card_types" : ["Physical", "Virtual"],
                                        "target_account_range": {
                                                    "country_code": "GB",
                                                    "start": "535737100002003",
                                                    "end": "535737101005999",
                                                    "reference": "account range reference"
                                                },
                                        "reference": "test reference"
                                        }""" clientId |> TextRequest

    let CreateCardProduct clientId  accessToken = 
        Http.RequestString ( 
                    partnerApiUrl + "card-products", 
                    headers = [ContentType HttpContentTypes.Json; Authorization ("Bearer " + accessToken)],
                    body = addCardProduct clientId )
                    