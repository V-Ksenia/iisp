using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using WEB_253504_VILKINA.DOMAIN.Entities;
using WEB_253504_VILKINA.UI.Services.JewelryService;

namespace WEB_253504_VILKINA.UI.Areas.Admin.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IJewelryService _jewelryService;

        public IndexModel(IJewelryService jewelryervice)
        {
            _jewelryService = jewelryervice;
        }

        public IList<Jewelry> Jewelry { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var jewelry = await _jewelryService.GetProductListAsync(pageSize: 1000);
            Jewelry = jewelry.Data.Items;

        }
    }
}
