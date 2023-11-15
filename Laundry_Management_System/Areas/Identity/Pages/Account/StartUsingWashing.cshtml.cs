using Laundry_Management_System.Areas.Identity.Data;
using Laundry_Management_System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection.PortableExecutable;
using WebApplication13.Filters;

public class StartUsingWashingModel : PageModel
{
    [AuthenticateUser]

    private readonly AuthDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly MachineService _machineService;

    [BindProperty]
    public int WashingMachineId { get; set; }

    [TempData]
    public string ErrorMessage { get; set; } // Property for error message

    public StartUsingWashingModel(AuthDbContext context, UserManager<ApplicationUser> userManager, MachineService machineService)
    {
        _context = context;
        _userManager = userManager;
        _machineService = machineService;
    }

    public void OnGet()
    {
        if (HttpContext.Request.Query.TryGetValue("wmachineId", out var wmachineId))
        {
            WashingMachineId = Convert.ToInt32(wmachineId);
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var currentUser = await _userManager.GetUserAsync(User);

        if (currentUser != null)
        {
            var userId = currentUser.Id;

            // Retrieve the washing machine based on WashingMachineId
            var wmachine = await _context.WashingMachines.FindAsync(WashingMachineId);

            if (wmachine != null && wmachine.Availability)
            {
                var cost = wmachine.Cost;

                // Check if the machine can be started
                if (_machineService.StartMachine(WashingMachineId, userId, cost))
                {
                    wmachine.Availability = false;
                    _context.Update(wmachine);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Set an error message if the machine cannot be started
                    ErrorMessage = "Insufficient balance to start the machine.";
                    return Page();
                }
            }
        }

        return RedirectToPage("/Account/MachineInfo");
    }
}
