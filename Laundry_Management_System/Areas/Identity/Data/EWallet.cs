using Laundry_Management_System.Areas.Identity.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace Laundry_Management_System.Models
{
    public class EWallet
    {
        [Key]
        public string UserId { get; set; } // This is the foreign key to ApplicationUser

        [Required]
        [Column(TypeName = "nvarchar(10)")]
        [Display(Name = "Card Number")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Card Number must be a 6-digit number.")]
        public string CardNumber { get; set; }

       

        [Required]
        [Column(TypeName = "money")]
        [Display(Name = "Balance")]
        [Range(1, 100, ErrorMessage = "Balance must be between 1 and 100.")]
        public decimal Balance { get; set; }

        public static bool IsValidBalance(string balance)
        {
            // Check if the balance is a numeric value
            if (Regex.IsMatch(balance, "^[0-9]+$"))
            {
                int balanceValue = int.Parse(balance);

                // Check if the balance is within the range of 1 to 100
                if (balanceValue >= 1 && balanceValue <= 100)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsValidAccountNumber(string accountnumber)
        {
            // Check if the balance is a numeric value
            if (Regex.IsMatch(accountnumber, "^[0-9]+$"))
            {
                                // Check if the balance is within the range of 1 to 100
                if (accountnumber.Length==6)
                {
                    return true;
                }
            }

            return false;
        }

        public ApplicationUser User { get; set; }

        // You can add more properties as needed
    }
}
