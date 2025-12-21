using System.ComponentModel.DataAnnotations;
using ProniaFrontToBack.Models.Base;

namespace ProniaFrontToBack.Models
{
    public class Slider : BaseEntity
    {
        [Required(ErrorMessage = "Name is required")]
        [
            StringLength(50, ErrorMessage = "Maximum length must be 50"),
            MinLength(3, ErrorMessage = "Minimum length must be 3")
        ]
        public string Name { get; set; }


        [Required(ErrorMessage = "Image is required")]
        [
            StringLength(300, ErrorMessage = "Maximum length must be 300"),
            MinLength(10, ErrorMessage = "Minimum length must be 10")
        ]
        public string Image { get; set; }


        [Required(ErrorMessage = "Description is required")]
        [
            StringLength(500, ErrorMessage = "Maximum length must be 500"),
            MinLength(10, ErrorMessage = "Minimum length must be 10")
        ]
        public string Description { get; set; }


        [Required(ErrorMessage = "Discount rate is required")]
        [Range(0, 100, ErrorMessage = "Discount rate must be between 0 and 100")]
        public int DiscountRate { get; set; }
    }
}