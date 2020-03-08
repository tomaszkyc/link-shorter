using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using LinkShorter.Models;
using LinkShorter.Models.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LinkShorter.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;

        private readonly EmailTemplate _emailTemplate;

        public ForgotPasswordModel(UserManager<IdentityUser> userManager, IEmailSender emailSender, EmailTemplate emailTemplate)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _emailTemplate = emailTemplate;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public String EmailSendingErrorMessage { get; set; }

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
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { code },
                    protocol: Request.Scheme);

                try
                {
                    
                   
                    ListDictionary replacements = new ListDictionary();
                    replacements.Add("@link-home@", HtmlEncoder.Default.Encode(Url.Action("Index", "Home", null, Request.Scheme, Request.Host.Value)));
                    replacements.Add("@link-reset-password@", HtmlEncoder.Default.Encode(callbackUrl));
                    replacements.Add("@link-privacy@", HtmlEncoder.Default.Encode(Url.Action("Privacy", "Home", null, Request.Scheme, Request.Host.Value)));
                    replacements.Add("@link-dashboard@", HtmlEncoder.Default.Encode(Url.Action("Index", "Dashboard", null, Request.Scheme, Request.Host.Value)));

                    string emailBody = await _emailTemplate.generateMailBody("wwwroot/resources/email/templates/forgot-password.html", replacements);
                    await _emailSender.SendEmailAsync(
                            Input.Email,
                            "Reset Password",
                            emailBody);
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }
                catch(Exception ex)
                {
                    EmailSendingErrorMessage = ex.Message;
                    return Page();
                }

            }

            return Page();
        }
    }
}
