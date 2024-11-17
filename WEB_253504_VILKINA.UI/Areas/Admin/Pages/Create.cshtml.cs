using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_253504_VILKINA.DOMAIN.Entities;
using WEB_253504_VILKINA.UI.Services.CategoryService;
using WEB_253504_VILKINA.UI.Services.FileService;
using WEB_253504_VILKINA.UI.Services.JewelryService;

namespace WEB_253504_VILKINA.UI.Areas.Admin.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IJewelryService _jewelryService;
        private readonly ICategoryService _categoryService;
        private readonly IFileService _fileService;



        public CreateModel(IJewelryService JewelryService, ICategoryService categoryService, IFileService fileService)
        {
            _jewelryService = JewelryService;
            _categoryService = categoryService;
            _fileService = fileService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
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



        [BindProperty]
        public Jewelry Jewelry { get; set; } = default!;

        [BindProperty]
        public IFormFile? Image { get; set; }

        public SelectList Categories { get; set; }



        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {

            var categoryResponse = await _categoryService.GetCategoryByIdAsync(Jewelry.CategoryId);
            if (!categoryResponse.Successfull || categoryResponse.Data == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid Category");
                return Page();
            }


            Jewelry.Category = categoryResponse.Data;


            if (Image != null)
            {
                try
                {

                    Jewelry.Image = Image.FileName; // Сохраняем URL изображения

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Error uploading image: " + ex.Message);
                    return Page();
                }
            }

            await _jewelryService.CreateProductAsync(Jewelry, Image);

            return RedirectToPage("./Index");
        }
    
    }
}
