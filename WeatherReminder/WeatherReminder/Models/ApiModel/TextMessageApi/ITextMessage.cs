using System.Threading.Tasks;

namespace WeatherReminder.Models.ApiModel.TextMessageApi
{
    public interface ITextMessage
    {
        string SendText(string countryCode, string phoneNumber, string message);
    }
}