using System.ComponentModel.DataAnnotations;

namespace ProniaFrontToBack.ViewModels.Account
{
    public record RegisterVM
    {
        [Required, MaxLength(25, ErrorMessage = "Name cannot exceed 25 characters")]
        public string Name { get; set; }
        [Required, MaxLength(25, ErrorMessage = "Surname cannot exceed 25 characters")]
        public string Surname { get; set; }
        [Required, MaxLength(25, ErrorMessage = "Username cannot exceed 25 characters")]
        public string UserName { get; set; }
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}