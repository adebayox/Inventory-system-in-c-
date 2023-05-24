using System;
namespace Inventory.Dtos.Laptops
{
	public class PatchLaptopDto
	{
        public string? SerialNumber { get; set; }
        public string? LaptopModel { get; set; }
        public string? Category { get; set; }
        public string? Previous_Owner { get; set; }
        public string? Assigned_To { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}

