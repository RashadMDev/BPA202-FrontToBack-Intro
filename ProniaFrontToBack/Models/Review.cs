using ProniaFrontToBack.Models.Base;

namespace ProniaFrontToBack.Models
{
    public class Review : BaseEntity
    {
        public string Comment { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}