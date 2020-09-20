module SampleData
    open ClientInterface

    let Address = ClientApi.Schemas.Address(
                        address_line1 =  "3 rue des cottages",
                        address_line2 =  Some "3 rue des cottages",
                        city =  "Paris",
                        zip = "75018",
                        state = Some "France",
                        country = "FR")

    let shippingInstructions = ClientApi.Schemas.ShippingInstruction(
                                    shipping_address = Address
                                )                    

    let AddAccount = ClientApi.Schemas.``add-account-request``("EUR",  Some "fake account")

    let AddAccountHolder accountId = ClientApi.Schemas.``add-individual-account-holder-request``(
                                        account_id = accountId,
                                        ``type`` = "individual",
                                        first_name = "etienne",
                                        last_name = "pierrot",
                                        billing_address = Address
                                        )                                  
    
    let AddVirtualCard accountId accountHolderId = ClientApi.Schemas.``add-virtual-card-request``(
                                                        account_id =  accountId,
                                                        account_holder_id = accountHolderId,
                                                        ``type`` = "Virtual"
                                                    )
    let AddPhysicalCard accountId accountHolderId = ClientApi.Schemas.``add-physical-card-request``(
                                                        account_id =  accountId,
                                                        account_holder_id = accountHolderId,
                                                        ``type`` = "Physical",
                                                        shipping_instructions = shippingInstructions
                                                    )                                        
