using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Laundry_Management_System.Areas.Identity.Data;
using Laundry_Management_System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using WebApplication13.Filters;

public class EditProfileModel : PageModel
{
    private readonly AuthDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<EditProfileModel> _logger;

    [AuthenticateUser]
    [BindProperty]
    [Required(ErrorMessage = "First Name is required.")]
    [StringLength(25, MinimumLength = 1, ErrorMessage = "First Name should be between 1 and 25 characters.")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Last Name is required.")]
    [StringLength(25, MinimumLength = 1, ErrorMessage = "Last Name should be between 1 and 25 characters.")]
    [Display(Name = "Last Name")]
    public string LastName { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid Email Address.")]
    [Display(Name = "Email")]
    public string Email { get; set; }

    public EditProfileModel(AuthDbContext context, UserManager<ApplicationUser> userManager, ILogger<EditProfileModel> logger)
    {
        _context = context;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task OnGetAsync()
    {
        try
        {
            // Retrieve the current user's profile data
            var currentUser = await _userManager.GetUserAsync(User);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving profile data on GET request.");
            throw; // Handle exception or redirect to an error page.
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            if (ModelState.IsValid)
            {
                // Update the user's profile data
                var currentUser = await _userManager.GetUserAsync(User);
                currentUser.First_Name = FirstName;
                currentUser.Last_Name = LastName;
                currentUser.Email = Email;

                var result = await _userManager.UpdateAsync(currentUser);
                if (result.Succeeded)
                {
                    // Profile updated successfully
                    return RedirectToPage("ProfileUpdated");
                }

                // Handle update errors
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating profile data on POST request.");
            throw; // Handle exception or redirect to an error page.
        }

        return Page();
    }
}
