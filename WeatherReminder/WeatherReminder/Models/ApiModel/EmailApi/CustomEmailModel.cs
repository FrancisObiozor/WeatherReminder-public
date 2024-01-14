using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherReminder.Models.ApiModel.EmailApi
{
    public class CustomEmailModel
    {
        public string Email { get; set; }
        public string Subject { get; set; }
        public string TemplateId { get; set; }
        public dynamic TemplateData { get; set; }
    }
}
