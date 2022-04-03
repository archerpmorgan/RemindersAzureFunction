using System;
using System.ComponentModel.Design;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Core;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http.Json;

namespace TimerTriggerReminders.Function
{
    public class AzureStorageService
    {
        private static string subscriptionId =
        "8b589f4d-d2e0-440f-8b15-c54222f204c1";
        private static string resourceGroupName =
        "armorgan-rg";
        private static string accountName =
        "selfteachdev";
        private static string cosmosDbEndpoint =
        "https://selfteachdev.documents.azure.com:443/";
        private static string databaseName =
        "reminders";
        private static string containerName =
        "reminders";

        // HttpClient is intended to be instantiated once, rather than per-use.
        private static HttpClient httpClient = new HttpClient();
        private static CosmosClient cosmosClient;

        public string personalEmail;
        public string sendgridKey;
        public string cosmosKey;
        public List<string> reminders;

        private ILogger logger;

        public async Task Initialize(ILogger logger)
        {
            reminders = new List<string>();
            this.logger = logger;
            SecretClientOptions options = new SecretClientOptions()
            {
                Retry =
                    {
                        Delay= TimeSpan.FromSeconds(2),
                        MaxDelay = TimeSpan.FromSeconds(16),
                        MaxRetries = 5,
                        Mode = RetryMode.Exponential
                    }
            };
            var secretClient = new SecretClient(new Uri("https://armorgankv.vault.azure.net/"), new DefaultAzureCredential(), options);

            personalEmail = secretClient.GetSecret("personalemail").Value.Value;
            sendgridKey = secretClient.GetSecret("sendgridkey").Value.Value;
            cosmosKey = secretClient.GetSecret("cosmosKey").Value.Value;
            

            cosmosClient = new CosmosClient(cosmosDbEndpoint, cosmosKey);
            await fetchReminders();
        }

        public AzureStorageService()
        {
        }

        private async Task fetchReminders(){
            var database = cosmosClient.GetDatabase(databaseName);
            var container = database.GetContainer(containerName);
            try
            {
                var response = await container.ReadItemAsync<ReminderSet>("2", new PartitionKey("2"));
                reminders.AddRange(response.Resource.content);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
        }
    }
}