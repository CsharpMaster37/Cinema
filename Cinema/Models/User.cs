using Microsoft.AspNetCore.Identity;

namespace Cinema.Models
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateofBirth { get; set; }
        public bool IsAdmin { get; set; }
    }
}
