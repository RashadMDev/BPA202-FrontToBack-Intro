using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProniaFrontToBack.Models;

namespace ProniaFrontToBack.ViewModels
{
    public record HomeVM
    {
        public List<Slider> Sliders { get; set; }
        public List<Product> Products { get; set; }
    }
}