using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using WeatherReminder.Models.ApiModel.ApiKeys;

namespace WeatherReminder.Models.ApiModel.EmailApi
{
    public class Email : IEmail
    {
        private readonly IConfiguration _configuration;
        private readonly IKeyVault _keyVault;

        public Email(IOptions<AuthMessageSenderOptions> optionsAccessor,
                     IConfiguration configuration,
                     IKeyVault keyVault)
        {
            Options = optionsAccessor.Value;
            _configuration = configuration;
            _keyVault = keyVault;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public Task SendEmailAsync(EmailModel emailModel)
        {

            var sendGridKey = _keyVault.ApiKeys.SendGridKey;
            return Execute(sendGridKey, emailModel);
        }

        private Task Execute(string apiKey, EmailModel emailModel)
        {
            var client = new SendGridClient(apiKey);

            var sendGridEmail = _keyVault.ApiKeys.SendGridEmail;

            var msg = new SendGridMessage()
            {
                From = new EmailAddress(sendGridEmail, Options.SendGridUser),
                Subject = emailModel.Subject,
                PlainTextContent = emailModel.Message,
                HtmlContent = emailModel.Message
            };
            msg.AddTo(new EmailAddress(emailModel.Email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }
    }
}
