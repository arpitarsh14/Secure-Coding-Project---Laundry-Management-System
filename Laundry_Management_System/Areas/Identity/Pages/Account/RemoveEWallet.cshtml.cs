using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Laundry_Management_System.Models;
using Microsoft.EntityFrameworkCore;
using Laundry_Management_System.Data;
using Microsoft.AspNetCore.Identity;
using Laundry_Management_System.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class RemoveEWalletModel : PageModel
{
    private readonly AuthDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    public string ErrorMessage { get; set; } // Error message property

    public RemoveEWalletModel(AuthDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task OnGet()
    {
        // Handle the GET request, if needed
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var currentUser = await _userManager.GetUserAsync(User);

        if (currentUser != null)
        {
            var eWallet = await _context.EWallets.SingleOrDefaultAsync(e => e.UserId == currentUser.Id);
            if (eWallet != null)
            {
                try
                {
                    _context.EWallets.Remove(eWallet);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    ErrorMessage = "An error occurred while removing the eWallet: " + ex.Message;
                    return Page();
                }
            }
            else
            {
                ErrorMessage = "eWallet not found.";
                return Page();
            }
        }
        else
        {
            ErrorMessage = "User not found.";
            return Page();
        }

        // Redirect to a confirmation page or any other page
        return RedirectToPage("ConfirmationPage");
    }
}
