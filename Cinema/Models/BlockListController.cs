using Microsoft.AspNetCore.Mvc;

namespace Cinema.Models
{
    public class BlockListController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
