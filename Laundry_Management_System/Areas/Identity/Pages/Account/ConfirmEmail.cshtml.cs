using System;

using System.Text;

using System.Text.Encodings.Web;

using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc.RazorPages;

using Laundry_Management_System.Areas.Identity.Data;

using Microsoft.AspNetCore.WebUtilities;



namespace Laundry_Management_System.Areas.Identity.Pages.Account

{

    [AllowAnonymous]

    public class ConfirmEmailModel : PageModel

    {

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly ILogger<ConfirmEmailModel> _logger;



        public ConfirmEmailModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<ConfirmEmailModel> logger)

        {

            _userManager = userManager;

            _signInManager = signInManager;

            _logger = logger;

        }



        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string code)

        {

            try

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

                if (!result.Succeeded)

                {

                    _logger.LogError("Error in confirming the user " + user.Email);

                    throw new InvalidOperationException($"Error confirming email for user with ID '{userId}':");

                }

                _logger.LogInformation("Email confirmation successful for the user " + user.Email);

                return RedirectToPage("Login");

            }

            catch (InvalidOperationException ex)

            {

                // Handle InvalidOperationException (e.g., log, display a message, etc.)



                StatusMessage = "Error confirming email. Please try again.";

                _logger.LogInformation("Error confirming email - Invalid operation" + ex.Message);

                ModelState.AddModelError("Email confirmation error", StatusMessage);

                return RedirectToPage("Login");

            }

            catch (TaskCanceledException ex)

            {

                StatusMessage = "Error confirming email. Please try again.";

                _logger.LogInformation("Error confirming email- failed task action " + ex.Message);

                ModelState.AddModelError("Email confirmation error", StatusMessage);

                return RedirectToPage("Login");

            }

            catch (ArgumentNullException ex)

            {

                StatusMessage = "Error confirming email. Please try again.";

                _logger.LogInformation("Error confirming email - null reference issue " + ex.Message);

                ModelState.AddModelError("Email confirmation error", StatusMessage);

                return RedirectToPage("Login");

            }

            catch (Exception ex)

            {

                StatusMessage = "Error confirming email. Please try again.";

                _logger.LogInformation("Error confirming email " + ex.Message);

                ModelState.AddModelError("Email confirmation error", StatusMessage);

                return RedirectToPage("Login");

            }





        }

    }

}

