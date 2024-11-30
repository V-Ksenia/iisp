using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_253504_VILKINA.DOMAIN.Models;
using WEB_253504_VILKINA.UI.Services.JewelryService;

namespace WEB_253504_VILKINA.UI.Controllers
{
    public class CartController : Controller
    {
        private readonly IJewelryService _jewelryService;
        private readonly Cart _cart;

        public CartController(IJewelryService jewelryService, Cart cart)
        {
            _jewelryService = jewelryService;
            _cart = cart;
        }
        public IActionResult Index()
        {
            return View(_cart);

        }

        [Authorize]
        [Route("[controller]/add/{id:int}")]
        public async Task<ActionResult> Add(int id, string returnUrl)
        {
            var data = await _jewelryService.GetProductByIdAsync(id);
            if (data.Successfull)
            {
                _cart.AddToCart(data.Data);
            }
            return Redirect(returnUrl);
        }

        [Authorize]
        [Route("[controller]/remove/{id:int}")]
        public async Task<ActionResult> Remove(int id, string returnUrl)
        {
            var data = await _jewelryService.GetProductByIdAsync(id);
            if (data.Successfull)
            {
                _cart.RemoveItems(data.Data.Id);
            }
            return Redirect(returnUrl);
        }

    }
}
