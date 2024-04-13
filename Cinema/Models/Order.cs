using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Models
{
    [Table("Order")]
    public class Order
    {
        [Key]
        [HiddenInput(DisplayValue = false)]// ID покупки
        public int Id { get; set; }

        [Required]
        [Range(0, 50000, ErrorMessage = "Недопустимая сумма")]
        [Display(Name = "Общая стоимость")]
        public int TotalPrice { get; set; }

        public DateTime Date { get; set; } // дата покупки
        public IEnumerable<OrderItem> Items { get; set; }       
    }
}
