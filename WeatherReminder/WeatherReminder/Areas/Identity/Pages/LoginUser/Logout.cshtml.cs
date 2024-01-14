using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using WeatherReminder.Models.HomepageModel;
using WeatherReminder.Models.WeatherReminderModel;

namespace WeatherReminder.Areas.Identity.Pages.LoginUser
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<WeatherReminderUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        private readonly IHomepageStats _homepageStats;

        public LogoutModel(SignInManager<WeatherReminderUser> signInManager, ILogger<LogoutModel> logger, IHomepageStats homepageStats)
        {
            _signInManager = signInManager;
            _logger = logger;
            _homepageStats = homepageStats;
        }

        public void OnGet()
        {
            _homepageStats.TimesVisited = 0;
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return RedirectToAction("Welcome", "Home");
                //return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Welcome", "Home");
                //return RedirectToPage();
            }
        }
    }
}
