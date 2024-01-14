using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherReminder.Models.UnconfirmedUsers
{
    public class UnconfirmedUser
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public DateTime AccountCreated { get; set; }
        public int ResendVerificationAttempts { get; set; }
    }
}
