using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_253504_VILKINA.DOMAIN.Entities;

namespace WEB_253504_VILKINA.DOMAIN.Models
{
    public class CartItem
    {
        public CartItem(Jewelry jewelries, int amount = 1)
        {
            Jewelry = jewelries;
            Amount = amount;
        }

        public Jewelry Jewelry { get; set; }
        public int Amount { get; set; }
        public void addAmount()
        {
            Amount += 1;
        }
        public double TotalPrice { get => Amount * Jewelry.Price; }

    }
}
