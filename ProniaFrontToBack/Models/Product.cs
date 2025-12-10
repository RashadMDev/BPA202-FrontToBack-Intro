using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProniaFrontToBack.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public decimal Rating { get; set; }
    }
}