using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherReminder.Models.TextMessageApi.VerifyCellApi
{
    public class CellVerificationStatus : ICellVerificationStatus
    {
        public string CellNumber { get; set; }
        public int VerificationAttempts { get; set; }
        public bool? CellUpdated { get; set; }

        //For homepage status
        public bool? InvalidPhoneNumberApiError { get; set; }
        public bool? MaxSendAttemptsApiError { get; set; }
        public bool? PhoneVerificationFailed { get; set; }
        public bool? PhoneVerificationSuccess { get; set; }
    }
}
