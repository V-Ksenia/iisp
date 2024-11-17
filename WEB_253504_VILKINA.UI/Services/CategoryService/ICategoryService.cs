using WEB_253504_VILKINA.DOMAIN.Entities;
using WEB_253504_VILKINA.DOMAIN.Models;

namespace WEB_253504_VILKINA.UI.Services.CategoryService
{
    public interface ICategoryService
    {
        public Task<ResponseData<List<Category>>> GetCategoryListAsync();
        public Task<ResponseData<Category>> GetCategoryByIdAsync(int categoryId);
    }
}
