using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Dynamic;
using WeatherReminder.Models.WeatherReminderModel;
using WeatherReminder.Models.ApiModel.EmailApi;

namespace WeatherReminder.Areas.Identity.Pages.LoginUser
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<WeatherReminderUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ICustomEmail _customEmail;

        public ForgotPasswordModel(UserManager<WeatherReminderUser> userManager,
                                    IEmailSender emailSender,
                                    ICustomEmail customEmail)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _customEmail = customEmail;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Profile/Password/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);

                var customEmailModel = new CustomEmailModel();
                customEmailModel.Email = Input.Email;
                customEmailModel.TemplateId = "d-eee9e63fae344ae9afa4c8b3ded26da5";
                customEmailModel.TemplateData = new
                {
                    ResetPassword = callbackUrl,
                    Email = user.Email
                };
                _customEmail.SendEmailAsync(customEmailModel);

                //await _emailSender.SendEmailAsync(
                //    Input.Email,
                //    "Reset Password",
                //    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}
