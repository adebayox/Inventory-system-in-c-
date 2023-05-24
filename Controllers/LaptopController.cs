using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Dtos.Laptops;
using Inventory.Models;
using Inventory.Services.LaptopService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Inventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LaptopController : ControllerBase
    {
        private readonly ILaptopService _laptopService;

        public LaptopController(ILaptopService laptopService)
        {
            _laptopService = laptopService;
        }

        //GET ALL LAPTOP
        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<GetLaptopDto>>>> Get()
        {
            return Ok(await _laptopService.GetAllLaptops());
        }

        //DELETE A LAPTOP
        [HttpDelete("{serialnumber}")]
        public async Task<ActionResult<ServiceResponse<List<GetLaptopDto>>>> Delete(string serialnumber)
        {
            var response = await _laptopService.DeleteLaptop(serialnumber);
            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        //GET A SINGLE LAPTOP BY USING SERIAL NUMBER
        [HttpGet("{serialnumber}")]
        public async Task<ActionResult<ServiceResponse<GetLaptopDto>>> GetSingle(string serialnumber)
        {
            return Ok(await _laptopService.GetLaptopById(serialnumber));
        }

        //ASSIGN A LAPTOP
        [HttpPost("assign-laptop")]
        public async Task<ActionResult<ServiceResponse<List<GetLaptopDto>>>> AddLaptop(AddLaptopDto newLaptop)
        {

            return Ok(await _laptopService.AddLaptop(newLaptop));
        }

        //RE-ASSIGN A LAPTOP
        [HttpPut("edit-assigned-laptop")]
        public async Task<ActionResult<ServiceResponse<GetLaptopDto>>> UpdatedLaptop(UpdateLaptopDto updatedLaptop)
        {
            var response = await _laptopService.UpdateLaptop(updatedLaptop);
            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        //EDIT AN ASSIGNED LAPTOP
        //[HttpPatch("edit-assigned-laptop/{serialNumber}")]
        //public async Task<ActionResult<ServiceResponse<GetLaptopDto>>> UpdateLaptop(string serialNumber, [FromBody] PatchLaptopDto updatedLaptop)
        //{
        //    var response = await _laptopService.PatchLaptop(serialNumber, updatedLaptop);

        //    if (response.Data == null)
        //    {
        //        return NotFound(response);
        //    }

        //    return Ok(response);
        //}


        //CONTROLLER TO GENERATE REPORT BASED ON DATE

        [HttpGet("GetReport")]
        public async Task<ActionResult<ServiceResponse<LaptopReportDto>>> GetReport(DateTime startDate, DateTime endDate)
        {
            var serviceResponse = await _laptopService.GetLaptopReport(startDate, endDate);
            return Ok(serviceResponse);
        }

        //CONTROLLER TO GENERATE REPORT AND DOWNLOAD AS EXCEL SHEET

        [HttpGet("GenerateReport")]
        public async Task<IActionResult> GenerateReport(DateTime startDate, DateTime endDate)
        {
            var reportStream = await _laptopService.GenerateLaptopReport(startDate, endDate);
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileName = "LaptopReport.xlsx";

            // Return the Excel file as a downloadable attachment
            return File(reportStream, contentType, fileName);
        }



    }
}
