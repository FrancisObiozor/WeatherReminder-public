using System.Threading.Tasks;

namespace WeatherReminder.Models.ApiModel.EmailApi
{
    public interface IEmail
    {
        Task SendEmailAsync(EmailModel email);
    }
}