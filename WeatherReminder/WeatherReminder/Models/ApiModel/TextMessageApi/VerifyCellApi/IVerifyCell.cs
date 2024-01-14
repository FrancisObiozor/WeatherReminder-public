namespace WeatherReminder.Models.ApiModel.TextMessageApi.VerifyCellApi
{
    public interface IVerifyCell
    {
        void SendVerificationCode(string countryCode, string phoneNumber);
        bool IsNumberValid(string countryCode, string phoneNumber, string code);
    }
}