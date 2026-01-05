namespace ProniaFrontToBack.Areas.Admin.ViewModels.Review
{
    public record UpdateReviewVM
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public int ProductId { get; set; }
    }
}