namespace ProniaFrontToBack.Areas.Admin.ViewModels.Product
{
    public record CreateProductVM
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal Rating { get; set; }
        public string SKU { get; set; }
        public IFormFile PrimaryImage { get; set; }
        public List<IFormFile>? Images { get; set; }
        public List<int> CategoryIds { get; set; }
        public List<int> TagIds { get; set; }
    }
}