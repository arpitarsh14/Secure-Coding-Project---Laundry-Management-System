using Laundry_Management_System.Areas.Identity.Data;
using Laundry_Management_System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using WebApplication13.Filters;

public class StartUsingDryerModel : PageModel
{
    [AuthenticateUser]

    private readonly AuthDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly MachineService _machineService;
    public string ErrorMessage { get; set; } // Error message property

    [BindProperty]
    public int DryerMachineId { get; set; }

    public StartUsingDryerModel(AuthDbContext context, UserManager<ApplicationUser> userManager, MachineService machineService)
    {
        _context = context;
        _userManager = userManager;
        _machineService = machineService;
    }

    public void OnGet()
    {
        if (HttpContext.Request.Query.TryGetValue("dmachineId", out var dmachineId))
        {
            DryerMachineId = Convert.ToInt32(dmachineId);
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var dmachine = await _context.DryerMachines.FindAsync(DryerMachineId);

        if (dmachine != null && dmachine.Availability)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var cost = dmachine.Cost;

            // Check if the user has sufficient balance
            if (_machineService.StartDryerMachine(DryerMachineId, TimeSpan.FromMinutes(2), currentUser.Id, cost))
            {
                dmachine.Availability = false;
                _context.Update(dmachine);
                await _context.SaveChangesAsync();
            }
            else
            {
                ErrorMessage = "Insufficient balance to start the dryer machine.";
                return Page();
            }
        }
        else
        {
            ErrorMessage = "Dryer machine not available.";
            return Page();
        }

        return RedirectToPage("/Account/MachineInfo");
    }
}
