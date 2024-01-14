using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherReminder.Models.PositionModel;

namespace WeatherReminder.Models.ApiModel.AutoCompleteApi
{
    public interface IAutoComplete
    {
        Task<List<string>> AutoCompleteOptions(CityStateCountry cityStateCountry);
        Task<bool> IsAnAutoCompleteOption(CityStateCountry cityStateCountry);
    }
}