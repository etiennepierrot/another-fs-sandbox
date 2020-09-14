module CardProduct
    
    open FSharp.Data
    open FSharp.Data.HttpRequestHeaders

    let partnerApiUrl = "http://localhost:5041/"
    
    let CreateCardProduct accessToken = Http.RequestString ( 
                                            partnerApiUrl + "card-products", 
                                            headers = [ContentType HttpContentTypes.Json; Authorization ("Bearer " + accessToken)],
                                            body = TextRequest """ 
                                            {
                                                "client_id": "cli_z5pganyndu2ezi23qe4dc3f3tm",
                                                "has_pci_clearance": "true",
                                                "maximum_card_lifetime": {
                                                    "units" : "Years",
                                                    "value": 3
                                                    },
                                                "allowed_card_types" : ["Physical", "Virtual"],
                                                "target_account_range": {
                                                            "country_code": "GB",
                                                            "start": "535737100001000",
                                                            "end": "535737101001999",
                                                            "reference": "account range reference"
                                                        },
                                                "reference": "test reference"
                                                }""")