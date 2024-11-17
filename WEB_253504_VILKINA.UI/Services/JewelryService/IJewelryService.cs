using WEB_253504_VILKINA.DOMAIN.Entities;
using WEB_253504_VILKINA.DOMAIN.Models;

namespace WEB_253504_VILKINA.UI.Services.JewelryService
{
    public interface IJewelryService
    {
        public Task<ResponseData<ProductListModel<Jewelry>>> GetProductListAsync(string? categoryNormalizedName = null, int pageNo = 1, int pageSize = -1);
        public Task<ResponseData<Jewelry>> GetProductByIdAsync(int id);
        public Task UpdateProductAsync(int id, Jewelry jewelry, IFormFile? formFile);
        public Task DeleteProductAsync(int id);
        public Task<ResponseData<Jewelry>> CreateProductAsync(Jewelry jewelry, IFormFile? formFile);
    }
}
