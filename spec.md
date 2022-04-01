# Design Spec

1. codebase in github
2. timer azure function runs every minute
3. random number in function dictates that only every so often the action actually does anything
4. sends and SMS to my phone using Azure Communication Services


- storage and ZCS connection string in key vault
- function app connects to key vault with managed identity
- database of reminders in storage account

WBAUP73579VK76105