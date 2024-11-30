using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;
using WEB_253504_VILKINA.DOMAIN.Entities;
using WEB_253504_VILKINA.DOMAIN.Models;
using WEB_253504_VILKINA.UI.Extensions;

namespace WEB_253504_VILKINA.UI.Services.CartService
{
    public class SessionCart : Cart
    {
        [JsonIgnore]
        public ISession? Session { get; set; }

        public static Cart GetCart(IServiceProvider services)
        {
            ISession? session = services.GetRequiredService<IHttpContextAccessor>().HttpContext?.Session;
            SessionCart cart = session?.Get<SessionCart>("Cart") ?? new SessionCart();
            cart.Session = session;
            return cart;
        }

        public override void AddToCart(Jewelry product)
        {
            base.AddToCart(product);
            Session?.Set("Cart", this);
        }
        public override void RemoveItems(int id)
        {
            base.RemoveItems(id);
            Session?.Set("Cart", this);
        }
        public override void ClearAll()
        {
            base.ClearAll();
            Session?.Remove("Cart");
        }
    }
}
