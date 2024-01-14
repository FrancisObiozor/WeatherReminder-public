using System.Threading.Tasks;
using WeatherReminder.Models.PositionModel;

namespace WeatherReminder.Models.ApiModel.GeocodeApi
{
    public interface IGeocodeLocation
    {
        Task<bool> VerifyLocationMatches(CityStateCountry cityStateCountry);
        Task<Coordinates> GetLatitudeLongitude(CityStateCountry location);
        Task<CityStateCountry> GetCityStateCountry(double latitude, double longitude);
    }
}