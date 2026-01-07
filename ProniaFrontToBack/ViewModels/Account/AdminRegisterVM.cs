using System.ComponentModel.DataAnnotations;

namespace ProniaFrontToBack.ViewModels.Account
{
    public record AdminRegisterVM
    {
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}