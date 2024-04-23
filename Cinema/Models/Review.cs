using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Models
{
    [Table("Reviews")]
    public class Review
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите текст отзыва")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Длина строки должна быть от 1 до 200 символов")]
        [Display(Name = "Текст отзыва")]
        public string Text {get; set; }

        [Required(ErrorMessage = "Пожалуйста, поставьте свою оценку!")]
        [Range(1, 5, ErrorMessage = "Оценка должна быть от 1 до 5")]
        [Display(Name = "Оценка")]
        public int Rating { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата отзыва")]
        public DateTime DatePosted { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string UserId { get; set; }

        [Display(Name = "Пользователь")]
        public string UserName { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int FilmId { get; set; }
        [Display(Name = "Фильм")]
        public Film Film { get; set; }
    }
}
