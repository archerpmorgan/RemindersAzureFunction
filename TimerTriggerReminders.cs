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
        public async Task Run([TimerTrigger("*/1 * * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var apiKey = Environment.GetEnvironmentVariable("NAME_OF_THE_ENVIRONMENT_VARIABLE_FOR_YOUR_SENDGRID_KEY");
            var personalEmail = Environment.GetEnvironmentVariable("PERSONAL_EMAIL");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(personalEmail, "Archer");
            var subject = "Test email";
            var to = new EmailAddress(personalEmail, "Archer");
            var plainTextContent = "Test content";
            var htmlContent = "<strong>Test content</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
