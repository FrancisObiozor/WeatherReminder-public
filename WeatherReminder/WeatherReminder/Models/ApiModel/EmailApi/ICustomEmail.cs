namespace WeatherReminder.Models.ApiModel.EmailApi
{
    public interface ICustomEmail
    {
        AuthMessageSenderOptions Options { get; }

        void SendEmailAsync(CustomEmailModel customEmailModel);
    }
}