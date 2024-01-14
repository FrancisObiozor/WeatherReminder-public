using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using WeatherReminder.Models.UnconfirmedUsers;
using WeatherReminder.Models.WeatherReminderModel;

namespace WeatherReminder.Areas.Identity.Pages.Profile.EmailUser
{
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<WeatherReminderUser> _userManager;
        private readonly IUnconfirmedUserRepository _unconfirmedUserRepository;

        public ConfirmEmailModel(UserManager<WeatherReminderUser> userManager,
                                IUnconfirmedUserRepository unconfirmedUserRepository)
        {
            _userManager = userManager;
            _unconfirmedUserRepository = unconfirmedUserRepository;
        }

        //[TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGet(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);

            StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";

            if (result.Succeeded)
            {
                var unconfirmedUser = _unconfirmedUserRepository.GetUser(user.Email);
                if (unconfirmedUser != null)
                {
                    _unconfirmedUserRepository.RemoveUser(unconfirmedUser);
                }
            }

            return Page();
        }
    }
}
