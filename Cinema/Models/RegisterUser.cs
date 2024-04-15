using System.ComponentModel.DataAnnotations;

namespace Cinema.Models
{
    public class RegisterUser
    {
        [Required]
        [Display(Name = "Имя пользователя")]
        public string Username { get; set; }

        [Display(Name = "Имя")]
        public string? FirstName { get; set; }
        [Display(Name = "Фамилия")]
        public string? LastName { get; set; }

        [Display(Name = "Дата рождения")]
        public DateTime? DateofBirth { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Повторите пароль")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
