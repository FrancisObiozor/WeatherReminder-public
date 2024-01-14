using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//Use this class as reference

namespace WeatherReminder.Models.ApiModel.EmailApi
{
    public class EmailModel
    {
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
