using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WeatherReminder.Models.ApiModel.ApiKeys;
using WeatherReminder.Models.PositionModel;

namespace WeatherReminder.Models.ApiModel.WeatherForecastApi.LocationApi
{
    public class LocationKey : ILocationKey
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IKeyVault _keyVault;

        public LocationKey(IHttpClientFactory clientFactory,
                           IKeyVault keyVault)
        {
            _clientFactory = clientFactory;
            _keyVault = keyVault;
        }

        public async Task<string> GetLocationKey(Coordinates coordinates)
        {
            var latitude = coordinates.Latitude;
            var longitude = coordinates.Longitude;
            var locationKey = new LocationKeyModel();

            var locationApiKey = _keyVault.ApiKeys.LocationKey;
            var request = new HttpRequestMessage(HttpMethod.Get, $"http://dataservice.accuweather.com/locations/v1/cities/geoposition/search?apikey={locationApiKey}" +
                                                                 $"q={latitude}%2C{longitude}");
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                locationKey.LocationKey = await response.Content.ReadFromJsonAsync<LocationKeyApi>();
                locationKey.Error = null;
                var results = locationKey.LocationKey.Key;
                return results;
            }
            else
            {
                locationKey.Error = $"There was an error getting the location: {response.ReasonPhrase}";
                return null;
            }
        }
    }


}
