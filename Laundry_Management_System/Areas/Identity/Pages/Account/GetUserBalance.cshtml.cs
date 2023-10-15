using Laundry_Management_System.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Laundry_Management_System.Data;

public class GetUserBalanceModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AuthDbContext _context;
    
    public GetUserBalanceModel(UserManager<ApplicationUser> userManager, AuthDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public decimal UserBalance { get; set; }

    public async Task<IActionResult> OnGet()
    {
        
        var currentUser = await _userManager.GetUserAsync(User);
        var eWallet = await _context.EWallets.FirstOrDefaultAsync(ew => ew.UserId == currentUser.Id);

        if (eWallet != null)
        {
            UserBalance=eWallet.Balance;
        }

        


        return Page();
    }
}
