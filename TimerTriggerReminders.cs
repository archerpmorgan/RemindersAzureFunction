using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;


namespace TimerTriggerReminders.Function
{
    public class TimerTriggerReminders
    {
        [FunctionName("TimerTriggerReminders")]
        public async Task Run([TimerTrigger("1 * * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            Random r = new Random();
            int rInt = r.Next(0, 1440);
            if (rInt != 100 && rInt != 1000) {
                // twice per day
                return;
            }

            var azureService = new AzureStorageService();
            await azureService.Initialize(log);

            //grab the reminder
            rInt = r.Next(0, azureService.reminders.Count);
            var reminder = azureService.reminders[rInt];

            var apiKey = azureService.sendgridKey;
            var personalEmail = azureService.personalEmail;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(personalEmail, "Archer");
            var subject = "Archer\'s Reminder Service";
            var to = new EmailAddress(personalEmail, "Archer");
            var plainTextContent = "Remember: " + reminder;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, "");
            var response = await client.SendEmailAsync(msg);
        }
    }
}
