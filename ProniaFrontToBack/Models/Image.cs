using ProniaFrontToBack.Models.Base;

namespace ProniaFrontToBack.Models
{
    public class Image : BaseEntity
    {
        public string Url { get; set; }
        public bool IsPrimary { get; set; } = false;
        public int ProductID { get; set; }
        public Product Product { get; set; }
    }
}