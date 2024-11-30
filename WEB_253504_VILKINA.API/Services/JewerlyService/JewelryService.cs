using Microsoft.EntityFrameworkCore;
using WEB_253504_VILKINA.API.Data;
using WEB_253504_VILKINA.DOMAIN.Entities;
using WEB_253504_VILKINA.DOMAIN.Models;

namespace WEB_253504_VILKINA.API.Services.JewerlyService
{
	public class JewelryService : IJewelryService
	{
		private readonly int _maxPageSize = 20;
		private readonly AppDbContext _context;
        private readonly ILogger _logger;
        public JewelryService(AppDbContext context, ILogger<JewelryService> logger)
		{
			_context = context;
            _logger = logger;
        }

		public async Task<ResponseData<Jewelry>> CreateProductAsync(Jewelry jewelry)
		{
            if (jewelry.Category != null)
            {
                // Присоединяем категорию к контексту, чтобы EF не пытался добавить её как новую
                _context.Categories.Attach(jewelry.Category);
            }

            var newProduct = await _context.Jewelrys.AddAsync(jewelry);
			await _context.SaveChangesAsync();
			return ResponseData<Jewelry>.Success(newProduct.Entity);
		}

		public async Task DeleteProductAsync(int id)
		{
            var jewDel = _context.Jewelrys.FirstOrDefault(d => d.Id == id);
            if (jewDel == null)
                throw new KeyNotFoundException("Medicine not found");
            _context.Remove(jewDel);
            await _context.SaveChangesAsync();
        }

		public async Task<ResponseData<Jewelry>> GetProductByIdAsync(int id)
		{
			var product = await _context.Jewelrys.FirstOrDefaultAsync(c => c.Id == id);
			if (product is null)
			{
				return ResponseData<Jewelry>.Error($"No such object with id : {id}");
			}
			return ResponseData<Jewelry>.Success(product);
		}

		public async Task<ResponseData<ProductListModel<Jewelry>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
		{
			var query = _context.Jewelrys.AsQueryable();
			var dataList = new ProductListModel<Jewelry>();

			if (pageSize > _maxPageSize)
				pageSize = _maxPageSize;

			query = query
				.Where(d => categoryNormalizedName == null || d.Category.NormalizedName.Equals(categoryNormalizedName));

			var count = await query.CountAsync();
			if (count == 0)
			{
				return ResponseData<ProductListModel<Jewelry>>.Success(dataList);
			}

			int totalPages = (int)Math.Ceiling(count / (double)pageSize);

			if (pageNo > totalPages)
			{
				return ResponseData<ProductListModel<Jewelry>>.Error("No such page");
			}

			dataList.Items = await query
									.OrderBy(c => c.Id)
									.Skip((pageNo - 1) * pageSize)
									.Take(pageSize)
									.ToListAsync();

			dataList.CurrentPage = pageNo;
			dataList.TotalPages = totalPages;

			return ResponseData<ProductListModel<Jewelry>>.Success(dataList);
		}

		public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
		{
			throw new NotImplementedException();
		}

		public async Task UpdateProductAsync(int id, Jewelry jewelry)
		{
			var product = await _context.Jewelrys.FirstOrDefaultAsync(c => c.Id == id);
			if (product is null)
				return;

			product.Price = jewelry.Price;
			product.Description = jewelry.Description;
			product.CategoryId = jewelry.CategoryId;
			product.Amount = jewelry.Amount;
			product.Image = jewelry.Image;

			_context.Entry(product).State = EntityState.Modified;
			await _context.SaveChangesAsync();
		}
	}
}
