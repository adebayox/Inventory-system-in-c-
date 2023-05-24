using System;
using Inventory.Dtos.Laptops;
using Inventory.Models;

namespace Inventory.Services.LaptopService
{
	public interface ILaptopService
	{
        object Laptops { get; }

        Task<ServiceResponse<List<GetLaptopDto>>> GetAllLaptops();

        Task<ServiceResponse<GetLaptopDto>> GetLaptopById(string serialnumber);

        Task<ServiceResponse<List<GetLaptopDto>>> AddLaptop(AddLaptopDto newLaptop);

        Task<ServiceResponse<GetLaptopDto>> UpdateLaptop(UpdateLaptopDto updatedLaptop);

        Task<ServiceResponse<List<GetLaptopDto>>> DeleteLaptop(string serialnumber);

        Task<ServiceResponse<LaptopReportDto>> GetLaptopReport(DateTime startDate, DateTime endDate);

        Task<MemoryStream> GenerateLaptopReport(DateTime startDate, DateTime endDate);

        Task<ServiceResponse<GetLaptopDto>> PatchLaptop(string serialNumber, PatchLaptopDto patchLaptop);
    }
}

