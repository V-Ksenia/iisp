using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_253504_VILKINA.UI.Models;

namespace WEB_253504_VILKINA.UI.Controllers
{
    public class Home : Controller
    {
        public IActionResult Index()
        {
            var listItems = new List<ListDemo>
            {
                new ListDemo { Id = 1, Name = "Item 1" },
                new ListDemo { Id = 2, Name = "Item 2" },
                new ListDemo { Id = 3, Name = "Item 3" }
            };

            var selectList = new SelectList(listItems, "Id", "Name");
            ViewBag.SelectList = selectList;

            return View(selectList);
        }
    }
}
