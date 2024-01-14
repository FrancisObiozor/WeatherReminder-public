using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WeatherReminder.Models.ApiModel.ApiKeys;

namespace WeatherReminder.Models.ApiModel.WeatherForecastApi.ForecastApi
{
    public class WeatherForecast : IWeatherForecast
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IKeyVault _keyVault;

        public WeatherForecast(IHttpClientFactory clientFactory,
                               IKeyVault keyVault)
        {
            _clientFactory = clientFactory;
            _keyVault = keyVault;
        }

        public async Task<DailyForecast[]> GetWeatherForcast(string locationKey)
        {
            var weatherForcast = new WeatherForecastModel();

            var weatherKey = _keyVault.ApiKeys.WeatherKey;
            var request = new HttpRequestMessage(HttpMethod.Get, "http://dataservice.accuweather.com/forecasts/v1/daily/5day/" +
                                                                $"{locationKey}?apikey={weatherKey}");

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                weatherForcast.WeatherForecast = await response.Content.ReadFromJsonAsync<WeatherForecastApi>();
                weatherForcast.Error = null;
                var results = weatherForcast.WeatherForecast.DailyForecasts;
                return results;
            }
            else
            {
                weatherForcast.Error = $"There was an error getting the location: {response.ReasonPhrase}";
                return null;
            }
        }

        public DailyForecast[] DailyForecastToArray(string dailyForecasts)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(DailyForecast[]));
            StringReader reader = new StringReader(dailyForecasts);
            var dailyForecastArray = (DailyForecast[])serializer.Deserialize(reader);
            return dailyForecastArray;
        }

        public string DailyForecastToString(DailyForecast[] dailyForecasts)
        {
            StringWriter writer = new StringWriter();
            XmlSerializer serializer = new XmlSerializer(typeof(DailyForecast[]));
            serializer.Serialize(writer, dailyForecasts);
            var dailyForecastString = writer.ToString();
            return dailyForecastString;
        }
    }
}
