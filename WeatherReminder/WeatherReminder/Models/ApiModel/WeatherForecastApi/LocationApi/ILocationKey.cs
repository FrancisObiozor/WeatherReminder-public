using System.Threading.Tasks;
using WeatherReminder.Models.PositionModel;

namespace WeatherReminder.Models.ApiModel.WeatherForecastApi.LocationApi
{
    public interface ILocationKey
    {
        Task<string> GetLocationKey(Coordinates coordinates);
    }
}