using System;
using System.Text;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using WeatherReminder.Models.ApiModel.ApiKeys;

namespace WeatherReminder.Models.ApiModel.TextMessageApi
{
    public class TextMessage : ITextMessage
    {
        private readonly IKeyVault _keyVault;

        public TextMessage(IKeyVault keyVault)
        {
            _keyVault = keyVault;
        }

        public string SendText(string countryCode, string phoneNumber, string textMessage)
        {
            var fullNumber = new StringBuilder(countryCode);

            fullNumber.Append(phoneNumber);

            var accountSid = _keyVault.ApiKeys.Twilio.AccountSid;

            var authToken = _keyVault.ApiKeys.Twilio.AuthToken;

            var twilioPhoneNumber = _keyVault.ApiKeys.Twilio.Phone;

            TwilioClient.Init(accountSid, authToken);

            try
            {
                var message = MessageResource.CreateAsync(
                                from: new Twilio.Types.PhoneNumber(twilioPhoneNumber),
                                to: new Twilio.Types.PhoneNumber(fullNumber.ToString()),
                                body: textMessage).Result;
                return message.Sid;
            }
            catch (AggregateException e)
            {
                return e.Message;
            }


        }
    }
}
