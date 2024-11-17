using Microsoft.EntityFrameworkCore;
using WEB_253504_VILKINA.DOMAIN.Entities;

namespace WEB_253504_VILKINA.API.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}

		public DbSet<Category> Categories { get; set; }
		public DbSet<Jewelry> Jewelrys { get; set; }
	}
}
