using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Models
{
    [Table("BlockListReviews")]
    public class ReviewBlockList
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата блокировки")]
        public DateTime Date { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string UserId { get; set; }

        [Display(Name = "Пользователь")]
        public string UserName { get; set; }
    }
}
