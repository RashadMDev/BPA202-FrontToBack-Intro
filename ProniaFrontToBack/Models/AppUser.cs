using Microsoft.AspNetCore.Identity;

namespace ProniaFrontToBack.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}