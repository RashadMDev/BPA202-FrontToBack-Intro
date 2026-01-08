using System.ComponentModel.DataAnnotations;

namespace ProniaFrontToBack.ViewModels.Account
{
    public record ResetVM
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        public string UserId { get; set; }
    }
}