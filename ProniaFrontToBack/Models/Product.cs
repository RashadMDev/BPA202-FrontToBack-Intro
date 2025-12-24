using System.ComponentModel.DataAnnotations;
using ProniaFrontToBack.Models.Base;

namespace ProniaFrontToBack.Models
{
    public class Product : BaseEntity
    {
        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Product name must be between 2 and 100 characters")]
        public string Name { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5")]
        public decimal Rating { get; set; }

        [Required(ErrorMessage = "SKU is required")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "SKU must be between 1 and 50 characters")]
        public string SKU { get; set; }

        public List<Review> Reviews { get; set; }
        public List<Image> Images { get; set; }
        [Required(ErrorMessage = "At least one category is required")]
        public List<Category> Categories { get; set; }
        [Required(ErrorMessage = "At least one tag is required")]
        public List<Tag> Tags { get; set; }
    }
}