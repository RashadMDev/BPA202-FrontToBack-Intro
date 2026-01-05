namespace ProniaFrontToBack.Areas.Admin.ViewModels.Product
{
    public record UpdateProductVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public IFormFile? PrimaryImage { get; set; }
        public List<IFormFile>? Images { get; set; }
        public List<ProductImagesVM>? OldImages { get; set; }
        public List<string>? ImagesUrls { get; set; }
        public List<int> CategoryIds { get; set; }
        public List<int> TagIds { get; set; }
    }
}