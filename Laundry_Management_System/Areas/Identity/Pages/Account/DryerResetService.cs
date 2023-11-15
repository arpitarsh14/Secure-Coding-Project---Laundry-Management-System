using Laundry_Management_System.Data;
using System;
using System.Linq;

public class DryerResetService
{
    private readonly MachineService _machineService;
    private readonly AuthDbContext _context;
    private readonly Timer _timer;
    private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(1); // Adjust the interval as needed

    public DryerResetService(MachineService machineService, AuthDbContext context)
    {
        _machineService = machineService;
        _context = context;
        _timer = new Timer(CheckAndResetDryer, null, TimeSpan.Zero, _checkInterval);
    }

    private void CheckAndResetDryer(object state)
    {
        // Get a list of all dryer machines
        var dryerMachines = _context.DryerMachines.ToList();

        foreach (var dryer in dryerMachines)
        {
            if (_machineService.IsMachineInUse(dryer.DryerMachineId))
            {
                // Check if the dryer has been in use for more than 2 minutes
                var usageDuration = _machineService.GetUsageDuration(dryer.DryerMachineId);
                if (usageDuration >= TimeSpan.FromMinutes(2))
                {
                    // Reset the dryer availability
                    _machineService.ResetMachineAvailability(dryer.DryerMachineId);
                }
            }
        }
    }
}
