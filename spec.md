# Design Spec

1. codebase in github
2. azure function runs every minute, but only succeeds, makes api calls, sends messages randomly 3-4 times per day
3. sends an email to my personal address using Sendgrid, or sends sms to my phone using twilio
4. secrets in key vault, access granted to function app through managed identity
5. reminder strings in cosmosdb
6. deployed through github action on commit to master or manually

<https://docs.microsoft.com/en-us/azure/cosmos-db/managed-identity-based-authentication>
<https://docs.microsoft.com/en-us/azure/key-vault/general/tutorial-net-create-vault-azure-web-app>
