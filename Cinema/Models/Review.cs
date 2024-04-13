using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Models
{
    public class Review
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите текст отзыва")]
        [Display(Name = "Текст отзыва")]
        public string Text {get; set; }

        [Range(1, 5, ErrorMessage = "Оценка должна быть от 1 до 5")]
        [Display(Name = "Оценка")]
        public int Rating { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата отзыва")]
        public DateTime DatePosted { get; set; }

        [ForeignKey("UserId")]
        [HiddenInput(DisplayValue = false)]
        public string UserId { get; set; }

        [Display(Name = "Пользователь")]
        public User User { get; set; }
        [ForeignKey("FilmId")]
        [HiddenInput(DisplayValue = false)]
        public int FilmId { get; set; }
        [Display(Name = "Фильм")]
        public Film Film { get; set; }
    }
}
