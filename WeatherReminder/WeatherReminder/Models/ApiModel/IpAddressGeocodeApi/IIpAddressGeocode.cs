using System.Threading.Tasks;

namespace WeatherReminder.Models.ApiModel.IpAddressGeocodeApi
{
    public interface IIpAddressGeocode
    {
        Task<IpAddressGeocodeModel> GetLocationFromIp();
    }
}