using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WeatherReminder.Models.ApiModel.ApiKeys;
using WeatherReminder.Models.ApiModel.GeocodeApi;
using WeatherReminder.Models.PositionModel;

namespace WeatherReminder.Models.ApiModel.AutoCompleteApi
{
    public class AutoComplete : IAutoComplete
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IGeocodeLocation _geocodeLocation;
        private readonly IKeyVault _keyVault;

        public AutoComplete(IHttpClientFactory clientFactory,
               IGeocodeLocation geocodeLocation,
               IKeyVault keyVault)
        {
            _clientFactory = clientFactory;
            _geocodeLocation = geocodeLocation;
            _keyVault = keyVault;
        }


        public async Task<List<string>> AutoCompleteOptions(CityStateCountry cityStateCountry)
        {
            var city = cityStateCountry.City;
            var state = cityStateCountry.State;
            var country = cityStateCountry.Country;
            var location = new AutoCompleteModel();
            var options = new List<string>();

            var autoCompleteKey = _keyVault.ApiKeys.AutoCompleteKey;

            var request = new HttpRequestMessage(HttpMethod.Get, $"https://maps.googleapis.com/maps/api/place/autocomplete/json?input={city}+{state}+{country}&key={autoCompleteKey}");

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                location.AutoCompleteOptions = await response.Content.ReadFromJsonAsync<AutoCompleteResults>();
                location.ErrorString = null;
                var results = location.AutoCompleteOptions.Predictions;
                var duplicates = new Dictionary<string, int>();

                foreach (var result in results)
                {


                    string[] addressComponents = result.Description.Split(',');
                    var length = addressComponents.Length;

                    if (length >= 3)
                    {
                        var predictedCity = addressComponents[length - 3];
                        var predictedState = addressComponents[length - 2];
                        var predictedCountry = addressComponents[length - 1];

                        var locationOption = $"{predictedCity}, {predictedState}, {predictedCountry}";

                        if (!duplicates.ContainsKey(locationOption))
                        {
                            options.Add(locationOption);
                            duplicates.Add(locationOption, 1);
                        }
                    }

                }
                return options;
            }
            else
            {
                location.ErrorString = $"There was an error getting the location options: {response.ReasonPhrase}";
                return null;
            }
        }



        public async Task<bool> IsAnAutoCompleteOption(CityStateCountry cityStateCountry)
        {
            var city = cityStateCountry.City;
            var state = cityStateCountry.State;
            var country = cityStateCountry.Country;
            var location = new AutoCompleteModel();
            var options = new List<string>();

            var autoCompleteKey = _keyVault.ApiKeys.AutoCompleteKey;

            var request = new HttpRequestMessage(HttpMethod.Get, $"https://maps.googleapis.com/maps/api/place/autocomplete/json?input={city}+{state}+{country}&key={autoCompleteKey}");

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                location.AutoCompleteOptions = await response.Content.ReadFromJsonAsync<AutoCompleteResults>();
                location.ErrorString = null;
                var results = location.AutoCompleteOptions.Predictions;

                foreach (var result in results)
                {
                    string[] addressComponents = result.Description.Split(',');
                    var length = addressComponents.Length;
                    var cleanCity = RemoveSpacesAndLowercase(city);
                    var cleanState = RemoveSpacesAndLowercase(state);
                    var cleanCountry = RemoveSpacesAndLowercase(country);
                    var cleanPredictedCity = RemoveSpacesAndLowercase(addressComponents[length - 3]);
                    var cleanPredictedState = RemoveSpacesAndLowercase(addressComponents[length - 2]);
                    var cleanPredictedCountry = RemoveSpacesAndLowercase(addressComponents[length - 1]);

                    if (length >= 3)
                    {
                        var cityMatch = cleanPredictedCity == cleanCity;
                        var stateMatch = cleanPredictedState == cleanState;
                        var countryMatch = cleanPredictedCountry == cleanCountry;

                        if (cityMatch && stateMatch && countryMatch)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;

        }

        private string RemoveSpacesAndLowercase(string str)
        {
            // Remove spaces
            string stringWithoutSpaces = str.Replace(" ", "");

            // Convert to lowercase
            string lowercaseString = stringWithoutSpaces.ToLower();

            return lowercaseString;
        }
    }


}


