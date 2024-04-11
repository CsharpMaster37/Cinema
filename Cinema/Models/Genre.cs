using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Models
{
    public class Genre
    {
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        [Required(ErrorMessage = "Пожалуйста введите название жанра")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 20 символов")]
        [Display(Name = "Жанр")]
        public string Name { get; set; } // название 
        public IEnumerable<Film> Films { get; set; }
    }
}
