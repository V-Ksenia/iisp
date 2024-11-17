using Microsoft.EntityFrameworkCore;
using WEB_253504_VILKINA.API.Data;
using WEB_253504_VILKINA.DOMAIN.Entities;
using WEB_253504_VILKINA.DOMAIN.Models;

namespace WEB_253504_VILKINA.API.Services.CategoryService
{
	public class CategoryService : ICategoryService
	{
		private readonly AppDbContext _context;
		public CategoryService(AppDbContext context)
		{
			_context = context;
		}

		public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
		{
			var categories = await _context.Categories.ToListAsync();
			if (!categories.Any() || categories is null)
			{
				return ResponseData<List<Category>>.Error("No categories in db");
			}

			return ResponseData<List<Category>>.Success(categories);
		}
        public async Task<ResponseData<Category>> GetCategoryByIdAsync(int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            return ResponseData<Category>.Success(category);
        }
    }
}
