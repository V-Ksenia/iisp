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
    public class DetailsModel : PageModel
    {
        private readonly IJewelryService _jewelryService;
        private readonly IFileService _fileService;

        public DetailsModel(IJewelryService jewelryService, IFileService fileService)
        {
            _jewelryService = jewelryService;
            _fileService = fileService;
        }

        [BindProperty]
        public Jewelry Jewelry { get; set; } = default!;

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
            return Page();
        }

       
    }
}
