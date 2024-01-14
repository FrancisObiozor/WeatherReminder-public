using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using System;
using System.Text.Json;

namespace WeatherReminder.Models.ApiModel.ApiKeys
{
    public class KeyVault : IKeyVault
    {
        private readonly IConfiguration _configuration;

        public ApiKeys ApiKeys { get; }

        public KeyVault(IConfiguration configuration)
        {
            _configuration = configuration;
            ApiKeys = new ApiKeys
            {
                SendGridUser = _configuration["SendGridUser"],
                SendGridKey = _configuration["SendGridKey"],
                SendGridEmail = _configuration["SendGridEmail"],
                ConnectionStringsWeatherReminderDbContextConnection = _configuration["ConnectionStrings:WeatherReminderDbContextConnection"],
                Twilio = new TwilioText
                {
                    AccountSid = _configuration["Twilio:AccountSid"],
                    AuthToken = _configuration["Twilio:AuthToken"],
                    PathServiceSid = _configuration["Twilio:PathServiceSid"],
                    Phone = _configuration["Twilio:Phone"]
                },
                AutoCompleteKey = _configuration["AutoCompleteKey"],
                GeocodeKey = _configuration["GeocodeKey"],
                IpAddressGeocodeKey = _configuration["IpAddressGeocodeKey"],
                WeatherKey = _configuration["WeatherKey"],
                LocationKey = _configuration["LocationKey"],
            };


            //try
            //{
            //    var keyVaultUrl = "https://weatherreminder.vault.azure.net/";
            //    var cred = new ChainedTokenCredential(new ManagedIdentityCredential(), new AzureCliCredential());
            //    SecretClient secretClient = new SecretClient(new Uri(keyVaultUrl), cred);
            //    var apiKeys = secretClient.GetSecretAsync("ApiKeys").Result.Value.Value;
            //    ApiKeys = JsonSerializer.Deserialize<ApiKeys>(apiKeys);
            //}
            //catch (Exception e)
            //{
            //    ApiKeys = new ApiKeys
            //    {
            //        SendGridUser = _configuration["SendGridUser"],
            //        SendGridKey = _configuration["SendGridKey"],
            //        SendGridEmail = _configuration["SendGridEmail"],
            //        ConnectionStringsWeatherReminderDbContextConnection = _configuration["ConnectionStrings:WeatherReminderDbContextConnection"],
            //        Twilio = new TwilioText
            //        {
            //            AccountSid = _configuration["Twilio:AccountSid"],
            //            AuthToken = _configuration["Twilio:AuthToken"],
            //            PathServiceSid = _configuration["Twilio:PathServiceSid"],
            //            Phone = _configuration["Twilio:Phone"]
            //        },
            //        AutoCompleteKey = _configuration["AutoCompleteKey"],
            //        GeocodeKey = _configuration["GeocodeKey"],
            //        IpAddressGeocodeKey = _configuration["IpAddressGeocodeKey"],
            //        WeatherKey = _configuration["WeatherKey"],
            //        LocationKey = _configuration["LocationKey"],
            //    };
            //}
        }
    }
}
