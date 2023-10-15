using Laundry_Management_System.Data;
using Microsoft.Extensions.Caching.Memory;

public class MachineService
{
    private readonly IMemoryCache _cache;
    private readonly AuthDbContext _context;
    private readonly TimeSpan MachineUsageTime = TimeSpan.FromMinutes(2);
    public MachineService(IMemoryCache cache, AuthDbContext context)
    {
        _cache = cache;
        _context = context;
    }

    /* public bool StartMachine(int machineId, TimeSpan usageTime)
     {
         if (!_cache.TryGetValue(machineId, out DateTime startTime))
         {
             startTime = DateTime.UtcNow;
             _cache.Set(machineId, startTime, new MemoryCacheEntryOptions
             {
                 AbsoluteExpirationRelativeToNow = usageTime
             });
             return true;
         }
         return false;
     }*/
    public bool StartMachine(int machineId, string userId, decimal cost)
    {
        if (!IsMachineInUse(machineId) && CheckBalance(userId, cost))
        {
            // Deduct balance from the user's eWallet
            DeductBalance(userId, cost);

            // Set the machine as in use
            _cache.Set(machineId, true, MachineUsageTime);

            return true;
        }

        return false;
    }

    public bool IsMachineInUse(int machineId)
    {
        if (_cache.TryGetValue(machineId, out bool inUse))
        {
            return inUse;
        }
        return false;
    }
    public void ResetMachinesIfExpired()
    {
        foreach (var machineId in _context.WashingMachines.Select(m => m.WashingMachineId).ToList())
        {
            if (IsMachineInUse(machineId))
            {
                ResetMachineAvailability(machineId);
            }
        }
    }

    public bool StartDryerMachine(int machineId, TimeSpan usageTime, string userId, decimal cost)
    {
        if (CheckBalance(userId, cost) && !_cache.TryGetValue(machineId, out DateTime startTime))
        {
            startTime = DateTime.UtcNow;
            _cache.Set(machineId, startTime, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = usageTime
            });

            // Deduct balance from the user's eWallet
            DeductBalance(userId, cost);

            return true;
        }
        return false;
    }


    public bool CheckBalance(string userId, decimal cost)
    {
        var balance = GetBalance(userId);
        return balance >= cost;
    }

    public decimal GetBalance(string userId)
    {
        var eWallet = _context.EWallets.FirstOrDefault(ew => ew.UserId == userId);

        if (eWallet != null)
        {
            return eWallet.Balance;
        }

        return 0; // Default to 0 balance if no wallet is found
    }



    public void ResetMachineAvailabilityIfTimeoutReached()
    {
        var machines = _context.WashingMachines.ToList(); // Fetch all machines

        foreach (var machine in machines)
        {
            if (IsMachineInUse(machine.WashingMachineId))
            {
                var usageDuration = GetUsageDuration(machine.WashingMachineId);
                if (usageDuration >= TimeSpan.FromMinutes(2))
                {
                    // Reset machine availability if it's been in use for over 2 minutes
                    ResetMachineAvailability(machine.WashingMachineId);
                }
            }
        }
    }
    public void DeductBalance(string userId, decimal amount)
    {
        var eWallet = _context.EWallets.FirstOrDefault(ew => ew.UserId == userId.ToString());

        if (eWallet != null && eWallet.Balance >= amount)
        {
            eWallet.Balance -= amount;
            _context.Update(eWallet);
            _context.SaveChanges();
        }
        
        
    }

   

    public void StopMachine(int machineId)
    {
        _cache.Remove(machineId);
    }

    public TimeSpan GetUsageDuration(int machineId)
    {
        if (_cache.TryGetValue(machineId, out DateTime startTime))
        {
            return DateTime.UtcNow - startTime;
        }
        return TimeSpan.Zero;
    }

    public void ResetMachineAvailability(int machineId)
    {
        // Reset the machine's availability in the database
        var machine = _context.WashingMachines.Find(machineId);
        if (machine != null)
        {
            machine.Availability = true;
            _context.Update(machine);
            _context.SaveChanges();
        }
    }
}
