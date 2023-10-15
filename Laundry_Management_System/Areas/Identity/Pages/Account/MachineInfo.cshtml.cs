using System.Collections.Generic;
using Laundry_Management_System.Areas.Identity.Data;
using Laundry_Management_System.Data; // Include your data context namespace
using Laundry_Management_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Laundry_Management_System.Areas.Identity.Pages.Account
{
    [Authorize]
    public class MachineInfoModel : PageModel
    {
        public EWallet EWallet { get; set; }
        private readonly AuthDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public MachineInfoModel(AuthDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<WashingMachine> WashingMachines { get; set; }
        public List<DryerMachine> DryerMachines { get; set; }
        public decimal UserBalance { get; private set; }


        public async Task OnGet()
        {
            // Retrieve washing machine and dryer machine data from the database
            WashingMachines = _context.WashingMachines.ToList();
            DryerMachines = _context.DryerMachines.ToList();
            
            
            // Set the UserBalance property // Set the UserBalance property
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
    }
}
