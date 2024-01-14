using Microsoft.AspNetCore.Http;
using WeatherReminder.Models.DataStorageModel;

namespace WeatherReminder.Models.ApiModel.AutoCompleteApi
{
    public class ValidateCityState
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IWeatherReminderUserRepository _weatherReminderUserRepository;

        public ValidateCityState(IHttpContextAccessor accessor,
                                 IWeatherReminderUserRepository weatherReminderUserRepository)
        {
            _accessor = accessor;
            _weatherReminderUserRepository = weatherReminderUserRepository;
        }
    }
}
