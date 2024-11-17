using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WEB_253504_VILKINA.DOMAIN.Entities;
using WEB_253504_VILKINA.UI.Services.CategoryService;
using WEB_253504_VILKINA.UI.Services.FileService;
using WEB_253504_VILKINA.UI.Services.JewelryService;

namespace WEB_253504_VILKINA.UI.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly IJewelryService _jewelryService;
        private readonly ICategoryService _categoryService;
        private readonly IFileService _fileService;



        public EditModel(IJewelryService JewelryService, ICategoryService categoryService, IFileService fileService)
        {
            _jewelryService = JewelryService;
            _categoryService = categoryService;
            _fileService = fileService;
        }

        [BindProperty]
        public Jewelry Jewelry { get; set; } = default!;

        [BindProperty]
        public IFormFile? Image { get; set; }

        public SelectList Categories { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jewelry = await _jewelryService.GetProductByIdAsync((int)id);
            if (jewelry == null)
            {
                return NotFound();
            }
            Jewelry = jewelry.Data;

            var categoryResponse = await _categoryService.GetCategoryListAsync(); // Ожидание асинхронного метода

            if (categoryResponse.Successfull) // Убедитесь, что используется правильное свойство Successful
            {
                var categories = categoryResponse.Data; // Получаем список категорий из ответа
                Categories = new SelectList(categories, "Id", "Name");
            }
            else
            {
                // Обработка ошибки получения категорий, если необходимо
                Categories = new SelectList(Enumerable.Empty<SelectListItem>()); // Пустой список в случае ошибки
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var categoryResponse = await _categoryService.GetCategoryByIdAsync(Jewelry.CategoryId);
            if (!categoryResponse.Successfull || categoryResponse.Data == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid Category");
                return Page();
            }

            Jewelry.Category = categoryResponse.Data;

            var existResponse = await _jewelryService.GetProductByIdAsync(Jewelry.Id);
            if (!existResponse.Successfull || existResponse.Data == null)
            {
                return NotFound();
            }

            var existingJewelry = existResponse.Data;

            if (Image == null)
            {
                Jewelry.Image = existingJewelry.Image;
            }
            else
            {
                Jewelry.Image = Image.FileName;

            }

            await _jewelryService.UpdateProductAsync(Jewelry.Id, Jewelry, Image);


            if (existingJewelry.Image != Jewelry.Image)    
            {
                await _fileService.DeleteFileAsync(existingJewelry.Image);
            }

            return RedirectToPage("./Index");
        }

    }

}

