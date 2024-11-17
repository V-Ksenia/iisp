using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB_253504_VILKINA.DOMAIN.Entities
{
    public class Jewelry
    {
        [Key]
        public int Id { get; set; }
        public short Amount { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public string? Image { get; set; }
        public int Price { get; set; }
    }
}
