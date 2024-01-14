using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using WeatherReminder.Models.DataStorageModel;
using WeatherReminder.Models.WeatherReminderModel;

namespace WeatherReminder.Areas.Identity.Pages.Profile.Account
{
    public class DeletePersonalDataModel : PageModel
    {
        private readonly UserManager<WeatherReminderUser> _userManager;
        private readonly SignInManager<WeatherReminderUser> _signInManager;
        private readonly ILogger<DeletePersonalDataModel> _logger;
        private readonly IWeatherReminderUserRepository _weatherReminderUserRepository;

        public DeletePersonalDataModel(
            UserManager<WeatherReminderUser> userManager,
            SignInManager<WeatherReminderUser> signInManager,
            ILogger<DeletePersonalDataModel> logger,
            IWeatherReminderUserRepository weatherReminderUserRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _weatherReminderUserRepository = weatherReminderUserRepository;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public bool RequirePassword { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = _weatherReminderUserRepository.GetUser(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            if (RequirePassword)
            {
                if (!await _userManager.CheckPasswordAsync(user, Input.Password))
                {
                    ModelState.AddModelError(string.Empty, "Incorrect password.");
                    return Page();
                }
            }

            _weatherReminderUserRepository.DeleteUserSettings(user);
            var result = await _userManager.DeleteAsync(user);
            var userId = await _userManager.GetUserIdAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred deleting user with ID '{userId}'.");
            }

            await _signInManager.SignOutAsync();

            _logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);

            return Redirect("~/");
        }
    }
}
