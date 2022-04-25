using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using System.Threading.Tasks;


namespace TimerTriggerReminders.Function
{
    public class TimerTriggerReminders
    {
        [FunctionName("TimerTriggerReminders")]

        //To run every minute, but only send a message about 3-4 times per day
        // 1440 minutes per day
        public async Task Run([TimerTrigger("* 1 6-20 * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            Random r = new Random();
            int rInt = r.Next(0, 16);
            if (rInt > 1) {
                return;
            }

            var azureService = new AzureStorageService();
            await azureService.Initialize(log);
            //grab the reminder
            rInt = r.Next(0, azureService.reminders.Count);
            var reminder = azureService.reminders[rInt];
            var plainTextContent = "Remember: " + reminder;

            log.LogInformation(reminder);

            // send reminder via sendgrid over email

            // var apiKey = azureService.sendgridKey;
            // var personalEmail = azureService.personalEmail;
            // var client = new SendGridClient(apiKey);
            // var from = new EmailAddress("archerpmorgan2@protonmail.com", "Archer");
            // var subject = "Archer\'s Reminder Service";
            // var to = new EmailAddress(personalEmail, "Archer");
            // var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, "");
            // var response = await client.SendEmailAsync(msg);

            // send reminder over sms            

            string accountSid = azureService.twiliosid;
            string authToken = azureService.twiliotoken;

            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body: plainTextContent,
                from: new Twilio.Types.PhoneNumber("+18025533331"),
                to: new Twilio.Types.PhoneNumber("+19713892940")
            );

            Console.WriteLine(message.Sid);
        }
    }
}
