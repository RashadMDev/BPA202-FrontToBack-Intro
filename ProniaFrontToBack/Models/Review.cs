using System.ComponentModel.DataAnnotations;
using ProniaFrontToBack.Models.Base;

namespace ProniaFrontToBack.Models
{
    public class Review : BaseEntity
    {
        [Required(ErrorMessage = "Comment is required")]
        [MaxLength(500, ErrorMessage = "Comment length must be max 500 characters")]
        [MinLength(3, ErrorMessage = "Comment length must be min 10 characters")]
        public string Comment { get; set; }
        [Required(ErrorMessage = "Product is required"),
         Range(1, int.MaxValue, ErrorMessage = "Please select a product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}