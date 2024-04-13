using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Models
{
    [Table("Film")]
    public class Film
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Пожалуйста введите название")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Длина строки должна быть от 1 до 50 символов")]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Пожалуйста введите страну выпуска")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Длина строки должна быть от 1 до 30 символов")]
        [Display(Name = "Страна")]
        public string Country { get; set; }      

        [Required]
        [Range(0, 50000, ErrorMessage = "Недопустимая цена")]
        [Display(Name = "Цена")]
        public int Price { get; set; }

        [Required]
        [Range(1895, 2024, ErrorMessage = "Недопустимый год")]
        [Display(Name = "Год выпуска")]
        public int Year { get; set; }

        [ForeignKey("GenreId")]
        [HiddenInput(DisplayValue = false)]
        public int? GenreId { get; set; }

        [Display(Name = "Жанр")]
        public Genre Genre { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string? ImageUrl { get; set; }
    }
}
