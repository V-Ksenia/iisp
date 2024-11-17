using Microsoft.AspNetCore.Mvc;
using WEB_253504_VILKINA.DOMAIN.Entities;
using WEB_253504_VILKINA.DOMAIN.Models;
using WEB_253504_VILKINA.UI.Services.CategoryService;

namespace WEB_253504_VILKINA.UI.Services.JewelryService
{
    public class MemoryJewelryService : IJewelryService
    {
        private readonly ICategoryService _categoryService;
        private readonly IConfiguration _config;
        List<Jewelry> _jewelryes;
        List<Category> _categories;
        public MemoryJewelryService([FromServices] IConfiguration config,
                                        ICategoryService categoryService)
        {
            _categoryService = categoryService;
            _categories = _categoryService.GetCategoryListAsync().Result.Data;
            _config = config;

            SetupData();
        }

        public Task<ResponseData<Jewelry>> CreateProductAsync(Jewelry jewelry, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Jewelry>> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<ProductListModel<Jewelry>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            var itemsPerPage = _config.GetValue<int>("ItemsPerPage");
            var items = _jewelryes.Where(p => categoryNormalizedName == null || p.Category.NormalizedName.Equals(categoryNormalizedName)).ToList();
            int totalPages = (int)Math.Ceiling((double)items.Count() / itemsPerPage);

            var pagedItems = new ProductListModel<Jewelry>
            {
                Items = items.Skip((pageNo - 1) * itemsPerPage).Take(itemsPerPage).ToList(),
                CurrentPage = pageNo,
                TotalPages = totalPages
            };

            var result = ResponseData<ProductListModel<Jewelry>>.Success(pagedItems);

            return Task.FromResult(result);
        }

        public Task UpdateProductAsync(int id, Jewelry jewelry, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public void SetupData()
        {
            _jewelryes = new List<Jewelry>
            {
                new Jewelry
                {
                    Id = 1,
                    Amount = 50,
                    Category = _categories[0],
                    CategoryId = 1,
                    Description = "Gold Flower Earrings",
                    Image = "images/earrings.png",
                    Price = 5,
                },
                new Jewelry
                {
                    Id = 2,
                    Amount = 50,
                    Category = _categories[1],
                    CategoryId = 2,
                    Description = "Gold Necklace",
                    Image = "images/necklace.png",
                    Price = 10,
                },
                new Jewelry
                {
                    Id = 3,
                    Amount = 50,
                    Category = _categories[2],
                    CategoryId = 3,
                    Description = "Gold Ribbon Ring",
                    Image = "images/ring.png",
                    Price = 4,
                },
                new Jewelry
                {
                    Id = 4,
                    Amount = 50,
                    Category = _categories[3],
                    CategoryId = 4,
                    Description = "Gold Thin Bracelet",
                    Image = "images/bracelet.png",
                    Price = 8,
                },
                new Jewelry
                {
                    Id = 5,
                    Amount = 50,
                    Category = _categories[4],
                    CategoryId = 5,
                    Description = "Diamond watches",
                    Image = "images/watches.png",
                    Price = 500,
                }
            };
        }

        Task<ResponseData<ProductListModel<Jewelry>>> IJewelryService.GetProductListAsync(string? categoryNormalizedName, int pageNo, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}
