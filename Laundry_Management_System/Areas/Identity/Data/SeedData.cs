using System;
using Laundry_Management_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Laundry_Management_System.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Laundry_Management_System.Areas.Identity.Data;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new AuthDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<AuthDbContext>>()))
        {
            // Check if there's any existing data in the tables
            if (!context.WashingMachines.Any() && !context.DryerMachines.Any())
            {
                // Seed WashingMachines
                context.WashingMachines.AddRange(
                    new WashingMachine { WMachine = "Washer 1", Cost = 2.5m, Availability = true },
                    new WashingMachine { WMachine = "Washer 2", Cost = 2.5m, Availability = true },
                    new WashingMachine { WMachine = "Washer 3", Cost = 2.5m, Availability = true },
                    new WashingMachine { WMachine = "Washer 4", Cost = 2.5m, Availability = true }
                );

                // Seed DryerMachines
                context.DryerMachines.AddRange(
                    new DryerMachine { DMachine = "Dryer 1", Cost = 3.0m, Availability = true },
                    new DryerMachine { DMachine = "Dryer 2", Cost = 3.0m, Availability = true },
                    new DryerMachine { DMachine = "Dryer 3", Cost = 3.0m, Availability = true },
                    new DryerMachine { DMachine = "Dryer 4", Cost = 3.0m, Availability = true }
                );

                context.SaveChanges();

               
            }
        }
    }

}
