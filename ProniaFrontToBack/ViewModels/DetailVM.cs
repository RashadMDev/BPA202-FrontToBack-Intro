using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProniaFrontToBack.Models;

namespace ProniaFrontToBack.ViewModels
{
    public record DetailVM
    {
        public List<Product> Products { get; set; }
        public Product Product { get; set; }
    }
}