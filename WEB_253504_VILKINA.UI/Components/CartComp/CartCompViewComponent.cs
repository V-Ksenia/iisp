using Microsoft.AspNetCore.Mvc;
using WEB_253504_VILKINA.DOMAIN.Models;

namespace WEB_253504_VILKINA.UI.Components.CartComp
{
    public class CartCompViewComponent : ViewComponent
    {
        private readonly Cart _cart;

        public CartCompViewComponent(Cart cart)
        {
            _cart = cart;
        }
        public IViewComponentResult Invoke()
        {

            return View(_cart);
        }
    }
}
