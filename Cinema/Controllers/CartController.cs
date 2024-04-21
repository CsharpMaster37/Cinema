using Cinema.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Controllers
{
    public class CartController : Controller
    {
        CinemaContext _db;
        UserManager<User> _userManager;
        public CartController(CinemaContext context, UserManager<User> userManager)
        {
            _db = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            if(!User.Identity.IsAuthenticated)
                return RedirectToAction("Login","User");
            var user = await _userManager.GetUserAsync(User);
            var cartItems = _db.Cart.Include(c => c.SelectFilm).Where(c => c.UserId == user.Id).Include(c=>c.SelectFilm.Genre).ToList();
            int sum = 0;
            foreach (var item in cartItems)
            {
                var film = _db.Films.Find(item.FilmId);
                item.SelectFilm = film;
                sum += item.SelectFilm.Price;
            }
            ViewBag.Sum = sum;
            return View(cartItems);
        }
        public async Task<IActionResult> Add(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var existingCartItem = await _db.Cart.FirstOrDefaultAsync(c => c.UserId == user.Id && c.FilmId == id);
            var newCartItem = new CartItem
            {
                UserId = user.Id,
                FilmId = id
            };
            _db.Cart.Add(newCartItem);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Film");
        }
        public IActionResult Delete(int id)
        {
            var cartItem = _db.Cart.Find(id);
            if (cartItem != null)
            {
                _db.Cart.Remove(cartItem);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
