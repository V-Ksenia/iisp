using WEB_253504_VILKINA.DOMAIN.Entities;
using WEB_253504_VILKINA.DOMAIN.Models;

namespace WEB_253504_VILKINA.UI.Services.CategoryService
{
    public class MemoryCategoryService : ICategoryService
    {
        public Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            List<Category> categories = new List<Category>
            {
                new Category { Id = 1, Name = "Earrings", NormalizedName = "earrings" },
                new Category { Id = 2, Name = "Necklaces", NormalizedName = "necklaces" },
                new Category { Id = 3, Name = "Rings", NormalizedName = "rings" },
                new Category { Id = 4, Name = "Bracelets", NormalizedName = "bracelets" },
                new Category { Id = 5, Name = "Watches", NormalizedName = "watches" }
            };

            var result = ResponseData<List<Category>>.Success(categories);

            return Task.FromResult(result);
        }

        Task<ResponseData<Category>> ICategoryService.GetCategoryByIdAsync(int categoryId)
        {
            throw new NotImplementedException();
        }

    }
}
