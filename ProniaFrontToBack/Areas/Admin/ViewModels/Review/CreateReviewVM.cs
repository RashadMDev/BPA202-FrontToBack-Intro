namespace ProniaFrontToBack.Areas.Admin.ViewModels.Review
{
    public record CreateReviewVM
    {
        public string Comment { get; set; }
        public int ProductId { get; set; }
    }
}