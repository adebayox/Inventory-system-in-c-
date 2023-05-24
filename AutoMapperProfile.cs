using System;
using AutoMapper;
using Inventory.Dtos.Laptops;
using Inventory.Models;

namespace Inventory
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
            CreateMap<Laptop, GetLaptopDto>();

            CreateMap<AddLaptopDto, Laptop>();

            CreateMap<UpdateLaptopDto, Laptop>();
        }
	}
}

