using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherReminder.ViewModels.EditViewModels
{
    public class EditPhoneViewModel
    {
        [Required(ErrorMessage = "Please enter your phone number")]
        [StringLength(25)]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        [Display(Name = "Update Phone Number")]
        public string Phone { get; set; }
    }
}
