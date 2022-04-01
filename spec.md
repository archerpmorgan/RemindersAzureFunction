# Design Spec

1. codebase in github
2. timer azure function runs every minute
3. random number in function dictates that only every so often the action actually does anything
    In the end I get about 2-3 reminders a day
4. sends an email to my personal address using Sendgrid


- storage and ZCS connection string in key vault
- function app connects to key vault with managed identity to access sendgrid, azure storage, and personal email address
<https://docs.microsoft.com/en-us/azure/cosmos-db/managed-identity-based-authentication>
- database of reminders in storage account

WBAUP73579VK76105