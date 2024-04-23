using Cinema.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Controllers
{
    public class UsersFilmsController : Controller
    {
        CinemaContext _db;
        UserManager<User> _userManager;
        public UsersFilmsController(CinemaContext context, UserManager<User> userManager)
        {
            _db = context;
            _userManager = userManager;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            User user = await _userManager.GetUserAsync(User);
            List<Order> orders = _db.Orders.Where(o => o.Status == "Выполнен" && o.UserId == user.Id).ToList();
            List<Film> films = new List<Film>();
            foreach (var order in orders)
            {
                var items = _db.Items.Where(i => i.OrderId == order.Id).Include(i => i.Film).Include(i=> i.Film.Genre).ToList();
                foreach (var item in items)
                {
                    films.Add(item.Film);
                }
            }
            return View(films);
        }
        [Authorize]
        public async Task<IActionResult> Search(string searchString)
        {
            User user = await _userManager.GetUserAsync(User);
            List<Order> orders = _db.Orders.Where(o => o.Status == "Выполнен" && o.UserId == user.Id).ToList();
            List<Film> films = new List<Film>();
            ViewBag.SearchString = searchString;
            foreach (var order in orders)
            {
                var items = _db.Items.Include(i => i.Film).Include(i => i.Film.Genre).Where(i => i.OrderId == order.Id).Where(i=>i.Film.Name.Contains(searchString)).ToList();
                foreach (var item in items)
                {
                    films.Add(item.Film);
                }
            }
            return View("Index", films);
        }
    }
}
