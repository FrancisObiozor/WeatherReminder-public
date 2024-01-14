using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Dynamic;
using WeatherReminder.Models.WeatherReminderModel;
using WeatherReminder.Models.ApiModel.EmailApi;

namespace WeatherReminder.Areas.Identity.Pages.Profile.EmailUser
{
    public partial class EmailModel : PageModel
    {
        private readonly UserManager<WeatherReminderUser> _userManager;
        private readonly SignInManager<WeatherReminderUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ICustomEmail _customEmail;

        public EmailModel(
            UserManager<WeatherReminderUser> userManager,
            SignInManager<WeatherReminderUser> signInManager,
            IEmailSender emailSender,
            ICustomEmail customEmail)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _customEmail = customEmail;
        }

        public string Username { get; set; }

        public string Email { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "New email")]
            public string NewEmail { get; set; }
        }

        private async Task LoadAsync(WeatherReminderUser user)
        {
            var email = await _userManager.GetEmailAsync(user);
            Email = email;
            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostChangeEmailAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var email = await _userManager.GetEmailAsync(user);
            if (Input.NewEmail != email)
            {
                CustomEmailModel customEmailModel = await ConfirmNewEmail(user);
                _customEmail.SendEmailAsync(customEmailModel);

                StatusMessage = "Confirmation link to change email sent. Please check your email.";
                return RedirectToPage();
            }

            StatusMessage = "Your email is unchanged.";
            return RedirectToPage();
        }

        private async Task<CustomEmailModel> ConfirmNewEmail(WeatherReminderUser user)
        {
            var userId = await _userManager.GetUserIdAsync(user);
            var code = _userManager.GenerateChangeEmailTokenAsync(user, Input.NewEmail).Result;
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmailChange",
                pageHandler: null,
                values: new { userId, email = Input.NewEmail, code },
                protocol: Request.Scheme);
            var customEmailModel = new CustomEmailModel();
            customEmailModel.Email = Input.NewEmail;
            customEmailModel.TemplateId = "d-80e12122ac994e8f91d643997db2610a";

            dynamic templateData = new ExpandoObject();
            templateData.ChangeEmail = callbackUrl;
            customEmailModel.TemplateData = templateData;
            return customEmailModel;
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId, code },
                protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(
                email,
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            StatusMessage = "Verification email sent. Please check your email.";
            return RedirectToPage();
        }
    }
}
