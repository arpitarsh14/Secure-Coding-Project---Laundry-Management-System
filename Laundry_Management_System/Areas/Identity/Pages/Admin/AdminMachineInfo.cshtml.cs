using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Laundry_Management_System.Areas.Identity.Data;
using Laundry_Management_System.Data;
using Laundry_Management_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Laundry_Management_System.Areas.Identity.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminMachineInfoModel : PageModel
    {
        private readonly AuthDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminMachineInfoModel(AuthDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public List<WashingMachine> WashingMachines { get; set; }
        public List<DryerMachine> DryerMachines { get; set; }
        public decimal UserBalance { get; private set; }

        public async Task<IActionResult> OnGet()
        {
            if (!User.IsInRole("Admin"))
            {
                // Redirect unauthorized users to the login page
                return RedirectToPage("/Identity/Account/Login");
            }

            // Retrieve washing machine data from the database


            WashingMachines = await _context.WashingMachines.ToListAsync();
            DryerMachines = await _context.DryerMachines.ToListAsync();
            // Set the UserBalance property
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                var existingEWallet = await _context.EWallets.FirstOrDefaultAsync(ew => ew.UserId == currentUser.Id);
                if (existingEWallet != null)
                {
                    UserBalance = existingEWallet.Balance;
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateWashingMachineCost(int machineId, decimal newCost)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var existingMachine = await _context.WashingMachines.FindAsync(machineId);
            if (existingMachine != null)
            {
                existingMachine.Cost = newCost;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Admin/AdminMachineInfo");
        }

        public async Task<IActionResult> OnPostUpdateDryerMachineCost(int machineId, decimal newCost)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var existingMachine = await _context.DryerMachines.FindAsync(machineId);
            if (existingMachine != null)
            {
                existingMachine.Cost = newCost;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Admin/AdminMachineInfo");
        }

        public async Task<IActionResult> OnPostRemoveWashingMachine(int machineId)
        {
            var machineToRemove = await _context.WashingMachines.FindAsync(machineId);
            if (machineToRemove != null)
            {
                _context.WashingMachines.Remove(machineToRemove);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Admin/AdminMachineInfo");
        }

        public async Task<IActionResult> OnPostRemoveDryerMachine(int machineId)
        {
            var machineToRemove = await _context.DryerMachines.FindAsync(machineId);
            if (machineToRemove != null)
            {
                _context.DryerMachines.Remove(machineToRemove);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Admin/AdminMachineInfo");
        }

        public async Task<IActionResult> OnPostAddNewWashingMachine(string newWMachine, decimal newWMCost)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var newMachine = new WashingMachine
            {
                WMachine = newWMachine,
                Cost = newWMCost,
                Availability = true // You can set the availability as needed
            };

            _context.WashingMachines.Add(newMachine);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Admin/AdminMachineInfo");
        }

        public async Task<IActionResult> OnPostAddNewDryerMachine(string newDMachine, decimal newDMCost)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var newMachine = new DryerMachine
            {
                DMachine = newDMachine,
                Cost = newDMCost,
                Availability = true // You can set the availability as needed
            };

            _context.DryerMachines.Add(newMachine);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Admin/AdminMachineInfo");
        }

    }
}
