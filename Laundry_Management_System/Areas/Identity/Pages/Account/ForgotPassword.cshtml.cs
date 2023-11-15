using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Laundry_Management_System.Areas.Identity.Data;
using Laundry_Management_System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Laundry_Management_System.Areas.Identity.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        [BindProperty]
        [Display(Name = "Old Password")]
        [Required(ErrorMessage = "Please enter your Old Password")]
        public string OldPassword { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please enter your New Password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string NewPassword { get; set; }

        private readonly AuthDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<ForgotPasswordModel> _logger; // Inject ILogger<T>

        public ForgotPasswordModel(AuthDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<ForgotPasswordModel> logger)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger; // Assign the injected logger
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var currentUser = await _userManager.GetUserAsync(User);

                    if (currentUser != null)
                    {
                        // Verify old password
                        var passwordCheck = await _userManager.CheckPasswordAsync(currentUser, OldPassword);

                        if (passwordCheck)
                        {
                            // Old password matches, proceed to update password
                            var changePasswordResult = await _userManager.ChangePasswordAsync(currentUser, OldPassword, NewPassword);

                            if (changePasswordResult.Succeeded)
                            {
                                // Password updated successfully
                                await _signInManager.RefreshSignInAsync(currentUser);
                                _logger.LogInformation("Password updated successfully for user: {UserName}", currentUser.UserName);
                                return RedirectToPage("ProfileUpdated"); // Redirect to a success page
                            }

                            foreach (var error in changePasswordResult.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Old password is incorrect.");
                            _logger.LogWarning("Failed password change attempt for user: {UserName}. Old password is incorrect.", currentUser.UserName);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "User not found.");
                        _logger.LogError("User not found while attempting password change.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing password change request.");
                throw; // Handle exception or redirect to an error page.
            }

            return Page();
        }
    }
}
