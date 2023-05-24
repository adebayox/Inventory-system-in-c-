using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Models
{
	public class Laptop
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InventoryId { get; set; }
        
        public string SerialNumber { get; set; }

        public string LaptopModel { get; set; }

        public string Category { get; set; }

        public string Assigned_To { get; set; }

        public string Previous_Owner { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}

