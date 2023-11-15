using Microsoft.AspNetCore.Mvc.RazorPages;
using Laundry_Management_System.Models;
using Microsoft.AspNetCore.Identity;
using Laundry_Management_System.Data;
using Laundry_Management_System.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using WebApplication13.Filters;
using Microsoft.Extensions.Logging; // Include the logging namespace

public class ManageeModel : PageModel
{
    private readonly AuthDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<ManageeModel> _logger; // Inject ILogger<T>

    public ManageeModel(AuthDbContext context, UserManager<ApplicationUser> userManager, ILogger<ManageeModel> logger)
    {
        _context = context;
        _userManager = userManager;
        _logger = logger; // Assign the injected logger
    }

    [AuthenticateUser]
    [BindProperty]
    public EWallet EWallet { get; set; }

    [BindProperty]
    public string newEmail { get; set; }

    public string Email { get; set; }
    public ApplicationUser ApplicationUser { get; set; }

    public decimal UserBalance { get; private set; }
    public string UserName { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string ErrorMessage { get; set; } // Error message property

    public async Task OnGet()
    {
        try
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var existingEWallet = await _context.EWallets.FirstOrDefaultAsync(ew => ew.UserId == currentUser.Id);

            UserBalance = existingEWallet?.Balance ?? 0;
            UserName = currentUser.UserName.ToString();
            FirstName = currentUser.First_Name.ToString();
            LastName = currentUser.Last_Name.ToString();
            Email = currentUser.Email.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching user information for {UserName}", User.Identity.Name);
            throw;
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                ErrorMessage = "User not found."; // Set an error message
                return Page();
            }

            var existingEWallet = await _context.EWallets.FirstOrDefaultAsync(ew => ew.UserId == currentUser.Id);

            if (existingEWallet == null)
            {
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
                existingEWallet.CardNumber = EWallet.CardNumber;
                existingEWallet.Balance += EWallet.Balance;

                _context.EWallets.Update(existingEWallet);
            }

            await _context.SaveChangesAsync();

            // Redirect to a confirmation page or any other page
            return RedirectToPage("ConfirmationPage");
        }
        catch (Exception ex)
        {
            ErrorMessage = "An error occurred: " + ex.Message; // Set an error message
            _logger.LogError(ex, "Error occurred while managing EWallet for user: {UserName}", User.Identity.Name);
            return Page();
        }
    }
}
