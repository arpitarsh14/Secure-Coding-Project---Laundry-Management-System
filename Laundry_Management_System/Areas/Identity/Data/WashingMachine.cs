using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Laundry_Management_System.Models
{
    public class WashingMachine
    {
        [Key]
        public int WashingMachineId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string WMachine { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public decimal Cost { get; set; }

        [Required]
        public bool Availability { get; set; }

        // You can add more properties as needed
    }
}
