// ManageModel.cshtml.cs
using Microsoft.AspNetCore.Mvc.RazorPages;
using Laundry_Management_System.Models;
using Microsoft.AspNetCore.Identity;
using Laundry_Management_System.Data;
using Laundry_Management_System.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

public class ManageeModel : PageModel
{
    [BindProperty]
    public EWallet EWallet { get; set; }
    private readonly AuthDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager; // Inject UserManager

    public ManageeModel(AuthDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

   // public void OnGet()
    //{
        // Handle the GET request, if needed
   // }
    public decimal UserBalance { get; private set; }

    public async Task OnGet()
    {
        var currentUser = await _userManager.GetUserAsync(User);
        var existingEWallet = await _context.EWallets.FirstOrDefaultAsync(ew => ew.UserId == currentUser.Id);
        UserBalance = existingEWallet?.Balance ?? 0; // Set the UserBalance property
    }




    public async Task<IActionResult> OnPostAsync()
    {
        var currentUser = await _userManager.GetUserAsync(User);

        var existingEWallet = await _context.EWallets
            .FirstOrDefaultAsync(ew => ew.UserId == currentUser.Id);

        if (existingEWallet == null)
        {
            // If no EWallet exists, create a new one
            var newEWallet = new EWallet
            {
                CardNumber = EWallet.CardNumber,
                Balance = EWallet.Balance,
                UserId = currentUser.Id
            };

            _context.EWallets.Add(newEWallet);
        }
        else
        {
            // If an EWallet exists, update card number and balance
            existingEWallet.CardNumber = EWallet.CardNumber;
            existingEWallet.Balance += EWallet.Balance; // Increase balance

            _context.EWallets.Update(existingEWallet);
        }

        await _context.SaveChangesAsync();

        // Redirect to a confirmation page or any other page
        return RedirectToPage("ConfirmationPage");
    }

}
