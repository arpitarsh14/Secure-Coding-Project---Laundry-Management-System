using Laundry_Management_System.Areas.Identity.Data;
using Laundry_Management_System.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Laundry_Management_System.Data;

public class AuthDbContext : IdentityDbContext<ApplicationUser>
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options)
        : base(options)
    {
    }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<WashingMachine> WashingMachines { get; set; }
    public DbSet<DryerMachine> DryerMachines { get; set; }
    public DbSet<EWallet> EWallets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure the relationship between EWallet and ApplicationUser
        modelBuilder.Entity<EWallet>()
            .HasOne(e => e.User) // EWallet has one User
            .WithOne(u => u.EWallet) // ApplicationUser has one EWallet
            .HasForeignKey<EWallet>(e => e.UserId); // Define the foreign key property

        // You can add other configuration as needed

        // Call the base OnModelCreating method
        base.OnModelCreating(modelBuilder);
    }
}

