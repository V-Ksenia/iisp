using Microsoft.AspNetCore.Mvc;

namespace WEB_253504_VILKINA.UI.Components
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cartInfo = new
            {
                TotalPrice = "00,0 руб",
                ItemCount = 0
            };

            return View(cartInfo);
        }
    }
}
