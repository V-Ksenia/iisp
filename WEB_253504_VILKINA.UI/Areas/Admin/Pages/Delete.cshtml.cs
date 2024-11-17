using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WEB_253504_VILKINA.DOMAIN.Entities;
using WEB_253504_VILKINA.UI.Services.FileService;
using WEB_253504_VILKINA.UI.Services.JewelryService;

namespace WEB_253504_VILKINA.UI.Areas.Admin.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly IJewelryService _jewelryService;
        private readonly IFileService _fileService;

        public DeleteModel(IJewelryService jewelryService, IFileService fileService)
        {
            _jewelryService = jewelryService;
            _fileService = fileService;
        }

        [BindProperty]
        public Jewelry Jewelrys { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jewelrys = await _jewelryService.GetProductByIdAsync((int)id);

            if (jewelrys == null)
            {
                return NotFound();
            }
            else
            {
                Jewelrys = jewelrys.Data;
            }
            return Page();

        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var jewelrys = await _jewelryService.GetProductByIdAsync((int)id);

            if (jewelrys.Data != null)
            {
                string file = jewelrys.Data.Image;

                Jewelrys = jewelrys.Data;
                await _jewelryService.DeleteProductAsync(jewelrys.Data.Id);
                await _fileService.DeleteFileAsync(file);

            }

            return RedirectToPage("./Index");
        }
    }
}
