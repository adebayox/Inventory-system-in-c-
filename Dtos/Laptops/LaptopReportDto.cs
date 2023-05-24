using System;
namespace Inventory.Dtos.Laptops
{
	public class LaptopReportDto
	{
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<GetLaptopDto> Laptops { get; set; }
    }
}

