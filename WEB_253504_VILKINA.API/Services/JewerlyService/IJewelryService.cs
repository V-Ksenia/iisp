using WEB_253504_VILKINA.DOMAIN.Entities;
using WEB_253504_VILKINA.DOMAIN.Models;

namespace WEB_253504_VILKINA.API.Services.JewerlyService
{
	public interface IJewelryService
	{
		public Task<ResponseData<ProductListModel<Jewelry>>> GetProductListAsync(string?
		categoryNormalizedName, int pageNo = 1, int pageSize = 3);
		public Task<ResponseData<Jewelry>> GetProductByIdAsync(int id);
		public Task UpdateProductAsync(int id, Jewelry jewelry);
		public Task DeleteProductAsync(int id);
		public Task<ResponseData<Jewelry>> CreateProductAsync(Jewelry jewelry);
		public Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile);
	}
}
