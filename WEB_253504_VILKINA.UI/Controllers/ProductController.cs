﻿using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using WEB_253504_VILKINA.DOMAIN.Entities;
using WEB_253504_VILKINA.UI.Services.CategoryService;
using WEB_253504_VILKINA.UI.Services.JewelryService;

namespace WEB_253504_VILKINA.UI.Controllers
{
    public class ProductController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IJewelryService _jewelryService;
        public ProductController(IJewelryService jewelryService, ICategoryService categoryService)
        {
            _categoryService = categoryService;
            _jewelryService = jewelryService;
        }
        public async Task<IActionResult> Index(string? category, int pageNo = 1)
        {
            var productResponse =await _jewelryService.GetProductListAsync(category, pageNo);
            if (!productResponse.Successfull)
                return NotFound(productResponse.ErrorMessage);

            var allCategories =
                await _categoryService.GetCategoryListAsync();
            if (!allCategories.Successfull)
            {
                return NotFound(allCategories.ErrorMessage);
            }

            var currentCategory = category == null ? "All" : allCategories.Data.FirstOrDefault(c => c.NormalizedName.Equals(category)).Name;

            ViewData["Categories"] = allCategories.Data;
            ViewData["CurrentCategory"] = currentCategory;

            return View(productResponse.Data);
        }
    }
}
