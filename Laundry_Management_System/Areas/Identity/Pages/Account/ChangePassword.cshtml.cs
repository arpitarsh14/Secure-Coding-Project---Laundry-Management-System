using Laundry_Management_System.Areas.Identity.Data;

using Laundry_Management_System.Data;

using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc.RazorPages;

using Microsoft.AspNetCore.WebUtilities;

using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Text;



namespace Laundry_Management_System.Areas.Identity.Pages.Account

{

    public class ChangePasswordModel : PageModel

    {

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly AuthDbContext _context;

        private readonly ILogger<ChangePasswordModel> _logger;

        public ChangePasswordModel(UserManager<ApplicationUser> userManager, AuthDbContext context, ILogger<ChangePasswordModel> logger)

        {

            _userManager = userManager;

            _context = context;

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



            /// <summary>

            ///   This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used

            ///   directly from your code. This API may change or be removed in future releases.

            /// </summary>

            [Required]

            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]

            [DataType(DataType.Password)]

            public string Password { get; set; }



            /// <summary>

            ///   This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used

            ///   directly from your code. This API may change or be removed in future releases.

            /// </summary>

            [DataType(DataType.Password)]

            [Display(Name = "Confirm password")]

            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]

            public string ConfirmPassword { get; set; }



            /// <summary>

            ///   This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used

            ///   directly from your code. This API may change or be removed in future releases.

            /// </summary>

            [Required]

            public string Code { get; set; }



        }



        public IActionResult OnGet(string code = null)

        {

            if (code == null)

            {

                return BadRequest("A code must be supplied for password reset.");

            }

            else

            {

                Input = new InputModel

                {

                    Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code))

                };

                return Page();

            }

        }



        public async Task<IActionResult> OnPostAsync()

        {

            try

            {

                if (!ModelState.IsValid)

                {

                    return Page();

                }



                var user = await _userManager.FindByEmailAsync(Input.Email);

                if (user == null)

                {

                    ModelState.AddModelError(string.Empty, "We don't recognize your username. Please try again.");

                    // Don't reveal that the user does not exist

                    _logger.LogWarning(Input.Email + "tired to reset password when they are not registered");

                    return RedirectToPage("./ResetPasswordConfirmation");

                }



                var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);

                if (result.Succeeded)

                {

                    _logger.LogInformation("Password successfuly reset for " + user.Email);

                    return RedirectToPage("./ResetPasswordConfirmation");

                }



                foreach (var error in result.Errors)

                {

                    ModelState.AddModelError(string.Empty, error.Description);

                }

                return Page();

            }

            catch (DbException e) {

                _logger.LogError($"DbUpdateConcurrencyException in ChangePasswordModel.OnPostAsync: {e.Message}");

                ModelState.AddModelError("ConcurrencyError", "A concurrency error occurred while processing your request.");

                return RedirectToPage("ChangePassword");

            }

            catch (DbUpdateConcurrencyException ex)

            {

                _logger.LogError($"DbUpdateConcurrencyException in ChangePasswordModel.OnPostAsync: {ex.Message}");

                ModelState.AddModelError("ConcurrencyError", "A concurrency error occurred while processing your request.");

                return RedirectToPage("ChangePassword");

            }

            catch (InvalidOperationException ex)

            {

                _logger.LogError($"InvalidOperationException in ChangePasswordModel.OnPostAsync: {ex.Message}");

                ModelState.AddModelError("InvalidOperation", "Invalid operation occurred while resetting the password.");

                return RedirectToPage("ChangePassword");

            }

            catch (Exception ex)

            {

                _logger.LogError($"An unexpected error occurred in ChangePasswordModel.OnPostAsync: {ex.Message}");

                ModelState.AddModelError("UnexpectedError", "An unexpected error occurred while resetting the password.");

                return RedirectToPage("ChangePassword");

            }

        }

    }

}