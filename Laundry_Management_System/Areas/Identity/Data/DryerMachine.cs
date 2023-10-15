using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Laundry_Management_System.Models
{
    public class DryerMachine
    {
        [Key]
        public int DryerMachineId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string DMachine { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public decimal Cost { get; set; }

        [Required]
        public bool Availability { get; set; }

        // You can add more properties as needed
    }
}
