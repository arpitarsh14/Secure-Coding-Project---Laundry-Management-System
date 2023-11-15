using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc.RazorPages;

using Microsoft.AspNetCore.WebUtilities;

using System.ComponentModel.DataAnnotations;

using System.Text.Encodings.Web;

using System.Text;

using Microsoft.AspNetCore.Identity.UI.Services;

using Laundry_Management_System.Areas.Identity.Data;
using Laundry_Management_System.Areas.Identity.Pages.Account;
using System.Net.Mail;
using Identity.Models;

namespace Laundry_Management_System.Areas.Identity.Pages.Account

{

    public class ResetPasswordModel : PageModel

    {

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly EmailHelper _emailSender;

        private readonly ILogger<ResetPasswordModel> _logger;

        public ResetPasswordModel(UserManager<ApplicationUser> userManager, EmailHelper emailSender, ILogger<ResetPasswordModel> logger)

        {

            _userManager = userManager;

            _emailSender = emailSender;

            _logger = logger;

        }



        /// <summary>

        ///   This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used

        ///   directly from your code. This API may change or be removed in future releases.

        /// </summary>

        [BindProperty]

        public InputModel Input { get; set; }



        /// <summary>

        ///   This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used

        ///   directly from your code. This API may change or be removed in future releases.

        /// </summary>

        public class InputModel

        {

            /// <summary>

            ///   This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used

            ///   directly from your code. This API may change or be removed in future releases.

            /// </summary>

            [Required]

            [EmailAddress]

            public string Email { get; set; }

        }



        public async Task<IActionResult> OnPostAsync()

        {

            try

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


                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    var callbackUrl = Url.Page(

                      "/Account/ChangePassword",

                      pageHandler: null,

                      values: new { area = "Identity", code },

                      protocol: Request.Scheme);



                    await _emailSender.SendEmail(

                      Input.Email,

                      "Reset Password",

                      $"Please reset your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");



                    _logger.LogInformation("Reset password email sent for the user " + user.Email);

                    return RedirectToPage("./ForgotPasswordConfirmation");

                }



                return Page();

            }

            catch (InvalidOperationException ex)

            {

                // Handle invalid operation exception (e.g., user not found)

                _logger.LogError($"Error in forgot password reset for user {Input.Email}: {ex.Message}");

                ModelState.AddModelError("Error", "Invalid operation. Please try again.");

                return RedirectToPage("./ResetPassword");

            }

            catch (SmtpException ex)

            {



                _logger.LogError($"SMTP error while sending reset password email for user {Input.Email}: {ex.Message}");

                ModelState.AddModelError("Error", "An error occurred while sending the reset password email. Please try again later.");

                return RedirectToPage("./ResetPassword");

            }

            catch (NullReferenceException ex)

            {



                _logger.LogError($"Null reference error while sending reset password email for user {Input.Email}: {ex.Message}");

                ModelState.AddModelError("Error", "An error occurred while sending the reset password email. Please try again later.");

                return RedirectToPage("./ResetPassword");

            }

            catch (Exception ex)

            {

                _logger.LogError($"error while sending reset password email for user {Input.Email}: {ex.Message}");

                ModelState.AddModelError("Error", "An error occurred while sending the reset password email. Please try again later.");

                return RedirectToPage("./ResetPassword");

            }

        }

    }

}