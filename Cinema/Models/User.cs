using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Models
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [Range(1900, 2023, ErrorMessage = "Недопустимый год")]
        public DateTime? DateofBirth { get; set; }
        public bool IsAdmin { get; set; }
        public bool Blocked { get; set; }
    }
}
