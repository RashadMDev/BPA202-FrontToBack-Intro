using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProniaFrontToBack.Models.Base;

namespace ProniaFrontToBack.Models
{
    public class Slider : BaseEntity
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public int DiscountRate { get; set; }
    }
}