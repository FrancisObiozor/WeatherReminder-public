using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherReminder.Models.ApiModel.ApiKeys;

namespace WeatherReminder.Models.ApiModel.EmailApi
{
    public class CustomEmail : ICustomEmail
    {
        private readonly IConfiguration _configuration;
        private readonly IKeyVault _keyVault;

        public CustomEmail(IConfiguration configuration,
                         IKeyVault keyVault)
        {
            _configuration = configuration;
            _keyVault = keyVault;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public void SendEmailAsync(CustomEmailModel customEmailModel)
        {
            var sendGridKey = _keyVault.ApiKeys.SendGridKey;
            var sendGridEmail = _keyVault.ApiKeys.SendGridEmail;

            var client = new SendGridClient(sendGridKey);
            var from = new EmailAddress(sendGridEmail, "Weather Reminder");
            var to = new EmailAddress(customEmailModel.Email, customEmailModel.Email);

            var templateId = customEmailModel.TemplateId;

            var msg = MailHelper.CreateSingleTemplateEmail(from, to, templateId, customEmailModel.TemplateData);

            client.SendEmailAsync(msg);
        }
    }
}
