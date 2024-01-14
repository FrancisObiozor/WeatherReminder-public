namespace WeatherReminder.Models.TextMessageApi.VerifyCellApi
{
    public interface ICellVerificationStatus
    {
        string CellNumber { get; set; }
        int VerificationAttempts { get; set; }
        bool? CellUpdated { get; set; }

        bool? InvalidPhoneNumberApiError { get; set; }
        bool? MaxSendAttemptsApiError { get; set; }
        bool? PhoneVerificationFailed { get; set; }
        bool? PhoneVerificationSuccess { get; set; }
    }
}