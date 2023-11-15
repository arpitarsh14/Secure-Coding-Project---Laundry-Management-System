using System.Collections.Generic;
using Laundry_Management_System.Areas.Identity.Data;
using Laundry_Management_System.Data;
using Laundry_Management_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication13.Filters;
using Microsoft.Extensions.Logging; // Include the logging namespace

namespace Laundry_Management_System.Areas.Identity.Pages.Account
{
    [AuthenticateUser]
    [Authorize]
    public class MachineInfoModel : PageModel
    {
        private readonly AuthDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<MachineInfoModel> _logger; // Inject ILogger<T>

        public MachineInfoModel(AuthDbContext context, UserManager<ApplicationUser> userManager, ILogger<MachineInfoModel> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger; // Assign the injected logger
        }

        public EWallet EWallet { get; set; }
        public List<WashingMachine> WashingMachines { get; set; }
        public List<DryerMachine> DryerMachines { get; set; }
        public decimal UserBalance { get; private set; }

        public async Task OnGet()
        {
            try
            {
                WashingMachines = await _context.WashingMachines.ToListAsync();
                DryerMachines = await _context.DryerMachines.ToListAsync();

                var currentUser = await _userManager.GetUserAsync(User);

                if (currentUser != null)
                {
                    var existingEWallet = await _context.EWallets.FirstOrDefaultAsync(ew => ew.UserId == currentUser.Id);

                    if (existingEWallet != null)
                    {
                        UserBalance = existingEWallet.Balance;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching machine information for user: {UserName}", User.Identity.Name);
                throw;
            }
        }
    }
}
