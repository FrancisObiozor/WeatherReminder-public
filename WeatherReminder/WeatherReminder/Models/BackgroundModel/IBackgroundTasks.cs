namespace WeatherReminder.Models.BackgroundModel
{
    public interface IBackgroundTasks
    {
        void SendUsersReminders();
        void UpdateUsersWeatherForecast();
        void DeleteUnconfirmedAccounts();
    }
}