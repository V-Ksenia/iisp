using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_253504_VILKINA.DOMAIN.Entities;

namespace WEB_253504_VILKINA.DOMAIN.Models
{
    public class Cart
    {
        public Dictionary<int, CartItem> CartItems { get; set; } = new();
        public virtual void AddToCart(Jewelry jewelries)
        {
            if (CartItems.ContainsKey(jewelries.Id))
            {
                CartItems[jewelries.Id].addAmount();
            }
            else
            {
                CartItems.Add(jewelries.Id, new CartItem(jewelries));
                
            }
        }
        public virtual void RemoveItems(int id)
        {
            CartItems.Remove(id);
        }
        public virtual void ClearAll()
        {
            CartItems.Clear();
        }
        public int Count { get => CartItems.Sum(item => item.Value.Amount); }
        public double TotalPrice
        {
            get => CartItems.Sum(item => item.Value.TotalPrice);
        }
    }
}
