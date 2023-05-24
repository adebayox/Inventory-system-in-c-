using System;
using Inventory.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Data
{
	public class ApplicationDbContext : DbContext
	{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Laptop> Laptops { get; set; }
    }
}

