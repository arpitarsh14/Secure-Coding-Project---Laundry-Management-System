using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Laundry_Management_System.Models;
using Microsoft.AspNetCore.Identity;
using static Laundry_Management_System.Areas.Identity.Data.ApplicationUser;

namespace Laundry_Management_System.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "Only alphabets are allowed.")]
    [StringLength(20, MinimumLength = 1, ErrorMessage = "Name length must be between 1 and 20 characters.")]
    public string? First_Name { get; set; }

    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "Only alphabets are allowed.")]
    [StringLength(20, MinimumLength = 1, ErrorMessage = "Name length must be between 1 and 20 characters.")]
    public string? Last_Name { get; set; }
    public EWallet EWallet { get; set; }

    public static bool IsValidFirstName(string FirstName)
    {
        //null check already present before reaching here
        Regex rx = new Regex("^([a-zA-Z]){1,51}$");
        if (!rx.IsMatch(FirstName))
        {
            return false;
        }
        return true;
    }
    public static bool IsValidLastName(string LastName)
    {
        //null check already present before reaching here
        Regex rx = new Regex("^([a-zA-Z]){1,51}$");
        if (!rx.IsMatch(LastName))
        {
            return false;
        }
        return true;
    }

    public static bool IsValidPassword(string Password)
    {
        if (Password.Length > 45 || Password.Length <6)
        {
            return false;
        }
        return true;
    }
    public static bool IsValidEmail(string Email)
    {
        if (Email.Length > 50)
        {
            return false;
        }
        Regex rx = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        if (!rx.IsMatch(Email))
        {
            return false;
        }
        return true;
    }

    public class ApplicationUsers
    {
        public List<ApplicationUser> ApplicationUser { get; set; }
    }

}

