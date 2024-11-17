using WEB_253504_VILKINA.DOMAIN.Entities;

namespace WEB_253504_VILKINA.API.Data
{
	public static class DbInitializer
	{
		public static async Task SeedData(WebApplication app)
		{
			var baseUrl = app.Configuration.GetSection("ApiBaseUrl").Value;

			List<Category> _categories = new List<Category>
			{
				new Category { Name = "Earrings", NormalizedName = "earrings" },
				new Category { Name = "Necklaces", NormalizedName = "necklaces" },
				new Category { Name = "Rings", NormalizedName = "rings" },
				new Category { Name = "Bracelets", NormalizedName = "bracelets" },
				new Category { Name = "Watches", NormalizedName = "watches" }
			};

			List<Jewelry> _jewelryes = new List<Jewelry>
			{
				new Jewelry
				{
					Amount = 50,
					Category = _categories[0],
					CategoryId = 1,
					Description = "Gold Flower Earrings",
					Image = $"{baseUrl}/Images/earrings.png",
					Price = 5,
				},
				new Jewelry
				{
					Amount = 50,
					Category = _categories[1],
					CategoryId = 2,
					Description = "Gold Necklace",
					Image = $"{baseUrl}/Images/necklace.png",
					Price = 10,
				},
				new Jewelry
				{
					Amount = 50,
					Category = _categories[2],
					CategoryId = 3,
					Description = "Gold Ribbon Ring",
					Image = $"{baseUrl}/Images/ring.png",
					Price = 4,
				},
				new Jewelry
				{
					Amount = 50,
					Category = _categories[3],
					CategoryId = 4,
					Description = "Gold Thin Bracelet",
					Image = $"{baseUrl}/Images/bracelet.png",
					Price = 8,
				},
				new Jewelry
				{
					Amount = 50,
					Category = _categories[4],
					CategoryId = 5,
					Description = "Diamond watches",
					Image = $"{baseUrl}/Images/watches.png",
					Price = 500,
				}
			};

			using var scope = app.Services.CreateScope();

			var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

			await context.Categories.AddRangeAsync(_categories);
			await context.Jewelrys.AddRangeAsync(_jewelryes);

			await context.SaveChangesAsync();
		}
	}
}
