using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using WeatherReminder.Models.ApiModel.ApiKeys;
using WeatherReminder.Models.ApiModel.GeocodeApi.GoogleGeocode;
using WeatherReminder.Models.DataStorageModel;
using WeatherReminder.Models.PositionModel;
using WeatherReminder.Models.WeatherReminderModel;

namespace WeatherReminder.Models.ApiModel.GeocodeApi
{
    public class GeocodeLocation : IGeocodeLocation
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IWeatherReminderUserRepository _weatherReminderUserRepository;
        private readonly IHttpContextAccessor _accessor;
        private readonly IKeyVault _keyVault;

        public GeocodeLocation(IHttpClientFactory clientFactory,
               IWeatherReminderUserRepository weatherReminderUserRepository,
               IHttpContextAccessor accessor,
               IKeyVault keyVault)
        {
            _clientFactory = clientFactory;
            _weatherReminderUserRepository = weatherReminderUserRepository;
            _accessor = accessor;
            _keyVault = keyVault;
        }

        public async Task<bool> VerifyLocationMatches(CityStateCountry cityStateCountry)
        {
            var city = cityStateCountry.City;
            var state = cityStateCountry.State;
            var country = cityStateCountry.Country;
            var geocodeKey = _keyVault.ApiKeys.GeocodeKey;

            var requestUrl = $"https://maps.googleapis.com/maps/api/geocode/json?address={city}%20{state}%20{country}&key={geocodeKey}";

            using (var client = _clientFactory.CreateClient())
            {
                var response = await client.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    var geocodeResults = await response.Content.ReadFromJsonAsync<GeocodeResults>();
                    var results = geocodeResults.Results;

                    foreach (var result in results)
                    {
                        string[] addressComponents = result.Formatted_address.Split(',');
                        var length = addressComponents.Length;
                        var foundCity = false;
                        var foundState = false;
                        var foundCountry = false;
                        if (length >= 3)
                        {
                            foundCity = addressComponents[length - 3] == city;
                            foundState = addressComponents[length - 2] == state;
                            foundCountry = addressComponents[length - 1] == country;
                        }

                        if (foundCity && foundState && foundCountry)
                        {
                            return true;
                        }
                    }
                    return false;
                }
                else
                {
                    var errorString = $"There was an error getting the location: {response.ReasonPhrase}";
                    return false;
                }
            }
        }


        public async Task<Coordinates> GetLatitudeLongitude(CityStateCountry location)
        {
            var coordinates = new Coordinates();
            var geocode = new GeocodeModel();


            var geocodeKey = _keyVault.ApiKeys.GeocodeKey;
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://maps.googleapis.com/maps/api/geocode/json?address={location.City}%20{location.State}%20{location.Country}&key={geocodeKey}");

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                geocode.LocationData = response.Content.ReadFromJsonAsync<GeocodeResults>().Result;
                geocode.ErrorString = null;
                var place = geocode.LocationData.Results[0].Geometry.Location;
                coordinates.Latitude = place.Lat.ToString();
                coordinates.Longitude = place.Lng.ToString();
            }
            else
            {
                geocode.ErrorString = $"There was an error getting our forecast: {response.ReasonPhrase}";
            }

            return coordinates;
        }


        public async Task<CityStateCountry> GetCityStateCountry(double latitude, double longitude)
        {
            var coordinates = new Coordinates();
            coordinates.Latitude = latitude.ToString();
            coordinates.Longitude = longitude.ToString();
            var geocode = new GeocodeModel();


            var geocodeKey = _keyVault.ApiKeys.GeocodeKey;
            var request = new HttpRequestMessage(HttpMethod.Get, "https://maps.googleapis.com/maps/api/geocode/json?" +
                $"latlng={coordinates.Latitude},{coordinates.Longitude}&key={geocodeKey}");

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                geocode.LocationData = response.Content.ReadFromJsonAsync<GeocodeResults>().Result;
                geocode.ErrorString = null;
                return extractCityStateCountry(geocode);
            }
            else
            {
                geocode.ErrorString = $"There was an error getting our forecast: {response.ReasonPhrase}";
            }

            return new CityStateCountry();
        }


        public CityStateCountry extractCityStateCountry(GeocodeModel geocode)
        {
            var address = "";

            if (geocode.LocationData.Results.Length > 1)
            {
                address = geocode.LocationData.Results[1].Formatted_address;
            }
            else
            {
                address = geocode.LocationData.Results[0].Formatted_address;

            }
            string[] addressComponents = address.Split(',');
            var length = addressComponents.Length;
            var cityStateCountry = new CityStateCountry
            {
                Country = addressComponents[length - 1],
                State = addressComponents[length - 2],
                City = addressComponents[length - 3],
            };
            return cityStateCountry;
        }



    }
}
