module SampleData
    open ClientInterface

    let Address = ClientApi.Schemas.Address(
                        address_line1 =  "3 rue des cottages",
                        address_line2 =  Some "3 rue des cottages",
                        city =  "Paris",
                        zip = "75018",
                        state = Some "France",
                        country = "FR")

    let AddAccount = ClientApi.Schemas.``add-account-request``("EUR",  Some "fake account")

    let AddAccountHolder accountId = ClientApi.Schemas.``add-individual-account-holder-request``(
                                        account_id = Some accountId,
                                        ``type`` = "individual",
                                        first_name = Some "etienne",
                                        last_name = Some "pierrot",
                                        billing_address = Address
                                    )                                   
    
    let AddCard accountId accountHolderId = ClientApi.Schemas.``add-card-request``(
                                                account_id =  accountId,
                                                account_holder_id = accountHolderId,
                                                ``type`` = "Virtual"
                                            )
