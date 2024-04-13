using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Models
{
    public class OrderItem
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int OrderId { get; set; }

        [ForeignKey("FilmId")]
        [HiddenInput(DisplayValue = false)]
        public int FilmId { get; set; }

        public Film Film { get; set; }
    }
}
