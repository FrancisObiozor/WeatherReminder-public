using System.Text;
using System.Text.RegularExpressions;
using Twilio;
using Twilio.Rest.Verify.V2.Service;
using WeatherReminder.Models.ApiModel.ApiKeys;

namespace WeatherReminder.Models.ApiModel.TextMessageApi.VerifyCellApi
{
    public class VerifyCell : IVerifyCell
    {
        private readonly IKeyVault _keyVault;

        public VerifyCell(IKeyVault keyVault)
        {
            _keyVault = keyVault;
        }

        public void SendVerificationCode(string countryCode, string phoneNumber)
        {
            string accountSid = _keyVault.ApiKeys.Twilio.AccountSid;
            string authToken = _keyVault.ApiKeys.Twilio.AuthToken;
            StringBuilder phone = new StringBuilder("+");
            phone.Append(Regex.Replace(countryCode, @"[^0-9]", ""));
            phone.Append(Regex.Replace(phoneNumber, @"[^0-9]", ""));

            TwilioClient.Init(accountSid, authToken);

            var verification = VerificationResource.Create(
                to: phone.ToString(),
                channel: "sms",
                pathServiceSid: _keyVault.ApiKeys.Twilio.PathServiceSid
            );

        }

        public bool IsNumberValid(string countryCode, string phoneNumber, string code)
        {
            string accountSid = _keyVault.ApiKeys.Twilio.AccountSid;
            string authToken = _keyVault.ApiKeys.Twilio.AuthToken;
            StringBuilder phone = new StringBuilder("+");
            phone.Append(Regex.Replace(countryCode, @"[^0-9]", ""));
            phone.Append(Regex.Replace(phoneNumber, @"[^0-9]", ""));
            bool isValid = false;

            TwilioClient.Init(accountSid, authToken);

            var verificationCheck = VerificationCheckResource.Create(
                to: phone.ToString(),
                code: code,
                pathServiceSid: _keyVault.ApiKeys.Twilio.PathServiceSid
            );

            if (verificationCheck.Status == "approved")
            {
                isValid = true;
            }

            return isValid;
        }
    }

}
