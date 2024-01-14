using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;
using WeatherReminder.Models.ApiModel.EmailApi;
using WeatherReminder.Models.DataStorageModel;
using WeatherReminder.Models.UnconfirmedUsers;
using WeatherReminder.Models.WeatherReminderModel;

namespace WeatherReminder.Areas.Identity.Pages.RegisterUser
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        private readonly UserManager<WeatherReminderUser> _userManager;
        private readonly IEmailSender _sender;
        private readonly IUnconfirmedUserRepository _unconfirmedUserRepository;
        private readonly ICustomEmail _customEmail;
        private readonly IWeatherReminderUserRepository _weatherReminderUserRepository;

        public RegisterConfirmationModel(UserManager<WeatherReminderUser> userManager,
                                        IEmailSender sender,
                                        IUnconfirmedUserRepository unconfirmedUserRepository,
                                        ICustomEmail customEmail,
                                        IWeatherReminderUserRepository weatherReminderUserRepository)
        {
            _userManager = userManager;
            _sender = sender;
            _unconfirmedUserRepository = unconfirmedUserRepository;
            _customEmail = customEmail;
            _weatherReminderUserRepository = weatherReminderUserRepository;
        }

        [BindProperty]
        public string Email { get; set; }

        public int ResendVerificationAttempts { get; set; }
        public int TotalAttempts { get; set; } = 3;

        public bool DisplayConfirmAccountLink { get; set; }

        public string EmailConfirmationUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
        {
            if (email == null)
            {
                return RedirectToPage("Profile/Home/Index", new { area = "Identity" });
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Unable to load user with email '{email}'.");
            }

            ResendVerificationAttempts = _unconfirmedUserRepository.GetUser(email).ResendVerificationAttempts;
            Email = email;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            var unconfirmedUser = _unconfirmedUserRepository.GetUser(Email);
            unconfirmedUser.ResendVerificationAttempts++;
            _unconfirmedUserRepository.UpdateUser(unconfirmedUser);


            var user = _weatherReminderUserRepository.GetUserByEmail(Email);
            if (unconfirmedUser.ResendVerificationAttempts == TotalAttempts)
            {
                return RedirectToPage("/RegisterUser/RegisterConfirmation", new { area = "Identity", email = user.Email });
            }
            else
            {
                CustomEmailModel customEmailModel = await ConstructConfirmationEmail(returnUrl, user);
                _customEmail.SendEmailAsync(customEmailModel);
                return RedirectToPage("/RegisterUser/RegisterConfirmation", new { area = "Identity", email = user.Email });
            }
        }

        private async Task<CustomEmailModel> ConstructConfirmationEmail(string returnUrl, WeatherReminderUser user)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var callbackUrl = Url.Page(
                "/Profile/EmailUser/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = user.Id, code, returnUrl },
                protocol: Request.Scheme);

            var customEmailModel = new CustomEmailModel();
            customEmailModel.Email = Email;
            customEmailModel.TemplateId = "d-80e12122ac994e8f91d643997db2610a";

            dynamic templateData = new ExpandoObject();
            templateData.ChangeEmail = callbackUrl;
            customEmailModel.TemplateData = templateData;
            return customEmailModel;
        }

        private async Task ConfirmAccountWithoutEmailVerification(string returnUrl, WeatherReminderUser user)
        {
            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            EmailConfirmationUrl = Url.Page(
                "/Profile/EmailUser/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId, code, returnUrl },
                protocol: Request.Scheme);
        }
    }
}
