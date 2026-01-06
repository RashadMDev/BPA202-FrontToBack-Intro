using System.ComponentModel.DataAnnotations;

namespace ProniaFrontToBack.ViewModels.Account
{
    public record LoginVM
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}