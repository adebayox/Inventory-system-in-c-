using System;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Security.Claims;
using AutoMapper;
using Inventory.Data;
using Inventory.Dtos.Laptops;
using Inventory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Inventory.Services.LaptopService
{
    public class LaptopService : ILaptopService
    {
        private static List<Laptop> laptops = new List<Laptop>
        {
            new Laptop(),

        };
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LaptopService(IMapper mapper, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public object Laptops => throw new NotImplementedException();

        //ADD LAPTOP TO THE DB
        public async Task<ServiceResponse<List<GetLaptopDto>>> AddLaptop(AddLaptopDto newLaptop)
        {
            var serviceResponse = new ServiceResponse<List<GetLaptopDto>>();
            var laptop = _mapper.Map<Laptop>(newLaptop);
            _context.Laptops.Add(laptop);
            await _context.SaveChangesAsync();

             serviceResponse.Data = await _context.Laptops
            .Select(c => _mapper.Map<GetLaptopDto>(c))
            .ToListAsync();

            return serviceResponse;
        }

        //DELETE LAPTOPS FROM DB

        public async Task<ServiceResponse<List<GetLaptopDto>>> DeleteLaptop(string serialnumber)
        {
            var serviceResponse = new ServiceResponse<List<GetLaptopDto>>();
            try
            {
                var laptop = await _context.Laptops
                    .FirstOrDefaultAsync(c => c.SerialNumber == serialnumber);
                if (laptop is null)

                    throw new Exception($"Laptop with Id '{serialnumber}' not found.");

                _context.Laptops.Remove(laptop);

                await _context.SaveChangesAsync();

                serviceResponse.Data =
                    await _context.Laptops
                        .Select(c => _mapper.Map<GetLaptopDto>(c)).ToListAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;

            }
            return serviceResponse;
        }


        //GET ALL LAPTOP IN DB

        public async Task<ServiceResponse<List<GetLaptopDto>>> GetAllLaptops()
        {
            var serviceResponse = new ServiceResponse<List<GetLaptopDto>>();
            var laptops = await _context.Laptops.ToListAsync();
            serviceResponse.Data = laptops.Select(c => _mapper.Map<GetLaptopDto>(c)).ToList();
            return serviceResponse;
        }

        //GET LAPTOP BY SERIAL NUMBER

        public async Task<ServiceResponse<GetLaptopDto>> GetLaptopById(string serialnumber)
        {
            var serviceResponse = new ServiceResponse<GetLaptopDto>();
            var laptops = await _context.Laptops
                .FirstOrDefaultAsync(c => c.SerialNumber == serialnumber);

            if (laptops == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Laptop not found.";
                return serviceResponse;
            }

            serviceResponse.Data = _mapper.Map<GetLaptopDto>(laptops);
            return serviceResponse;
        }

        //Generate report (custom date range)
        public async Task<ServiceResponse<LaptopReportDto>> GetLaptopReport(DateTime startDate, DateTime endDate)
        {
            var serviceResponse = new ServiceResponse<LaptopReportDto>();

            try
            {
                var laptops = await _context.Laptops
                    .Where(l => l.CreatedAt >= startDate && l.CreatedAt <= endDate)
                    .ToListAsync();

                var laptopReportDto = new LaptopReportDto
                {
                    StartDate = startDate,
                    EndDate = endDate,
                    Laptops = _mapper.Map<List<GetLaptopDto>>(laptops)
                };

                serviceResponse.Data = laptopReportDto;
            }
            catch (Exception ex)
            {
                // Handle any exceptions and set appropriate error message
                serviceResponse.Success = false;
                serviceResponse.Message = "Error occurred while retrieving the laptop report.";
                // You can log the exception for further investigation
                // logger.LogError(ex, "Error occurred while retrieving the laptop report.");
            }

            return serviceResponse;
        }

        //GENERATE REPORT AND DOWNLOAD EXCEL SHEET
        public async Task<MemoryStream> GenerateLaptopReport(DateTime startDate, DateTime endDate)
        {
            var serviceResponse = await GetLaptopReport(startDate, endDate);

            if (!serviceResponse.Success)
            {
                // Handle error response
                // You can throw an exception or handle the error in your own way
                throw new Exception(serviceResponse.Message);
            }

            var laptops = serviceResponse.Data.Laptops;

            // Create an Excel package
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Laptop Report");

                // Set headers
                worksheet.Cells[1, 1].Value = "Serial Number";
                worksheet.Cells[1, 2].Value = "Laptop Model";
                worksheet.Cells[1, 3].Value = "Category";
                worksheet.Cells[1, 4].Value = "Assigned To";
                worksheet.Cells[1, 5].Value = "Previous Owner";
                worksheet.Cells[1, 6].Value = "Created At";

                // Fill data
                for (var i = 0; i < laptops.Count; i++)
                {
                    var laptop = laptops[i];
                    var row = i + 2; // Start from row 2
                    worksheet.Cells[row, 1].Value = laptop.SerialNumber;
                    worksheet.Cells[row, 2].Value = laptop.LaptopModel;
                    worksheet.Cells[row, 3].Value = laptop.Category;
                    worksheet.Cells[row, 4].Value = laptop.Assigned_To;
                    worksheet.Cells[row, 5].Value = laptop.Previous_Owner;
                    worksheet.Cells[row, 6].Value = laptop.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss");
                }

                // Format the header row
                using (var range = worksheet.Cells[1, 1, 1, 6])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                }

                // Auto-fit columns
                worksheet.Cells.AutoFitColumns();

                // Generate the file stream
                var stream = new MemoryStream(package.GetAsByteArray());

                //// Return the Excel file as a downloadable stream
                //return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "laptop_report.xlsx");

                return stream;
            }
        }

        // UPDATE A LAPTOP ASSIGNED
        public async Task<ServiceResponse<GetLaptopDto>> UpdateLaptop(UpdateLaptopDto updatedLaptop)
        {
            var serviceResponse = new ServiceResponse<GetLaptopDto>();
            try
            {
                var laptop
                        = await _context.Laptops
                        .FirstOrDefaultAsync(c => c.SerialNumber == updatedLaptop.SerialNumber);
                if (laptop is null)
                    throw new Exception($"Laptop with Id '{updatedLaptop.SerialNumber}'not found.");

                laptop.SerialNumber = updatedLaptop.SerialNumber;
                laptop.LaptopModel = updatedLaptop.LaptopModel;
                laptop.Category = updatedLaptop.Category;
                laptop.Previous_Owner = updatedLaptop.Previous_Owner;
                laptop.Assigned_To = updatedLaptop.Assigned_To;
                laptop.CreatedAt = updatedLaptop.CreatedAt;

                await _context.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<GetLaptopDto>(laptop);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }


        //EDIT AN ASSIGNED LAPTOP
        public async Task<ServiceResponse<GetLaptopDto>> PatchLaptop(string serialNumber, PatchLaptopDto patchLaptop)
        {
            var serviceResponse = new ServiceResponse<GetLaptopDto>();

            try
            {
                var laptop = await _context.Laptops.FirstOrDefaultAsync(c => c.SerialNumber == serialNumber);

                if (laptop == null)
                {
                    throw new Exception($"Laptop with Serial Number '{serialNumber}' not found.");
                }

                if (patchLaptop.SerialNumber != null)
                {
                    laptop.SerialNumber = patchLaptop.SerialNumber;
                }

                if (patchLaptop.LaptopModel != null)
                {
                    laptop.LaptopModel = patchLaptop.LaptopModel;
                }

                if (patchLaptop.Category != null)
                {
                    laptop.Category = patchLaptop.Category;
                }

                if (patchLaptop.Previous_Owner != null)
                {
                    laptop.Previous_Owner = patchLaptop.Previous_Owner;
                }

                if (patchLaptop.Assigned_To != null)
                {
                    laptop.Assigned_To = patchLaptop.Assigned_To;
                }

                if (patchLaptop.CreatedAt != null)
                {
                    laptop.CreatedAt = patchLaptop.CreatedAt.Value;
                }

                await _context.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<GetLaptopDto>(laptop);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }


    }
}

