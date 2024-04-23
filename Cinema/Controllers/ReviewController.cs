using Cinema.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Controllers
{
    public class ReviewController : Controller
    {
        CinemaContext _db;
        UserManager<User> _userManager;
        public ReviewController(CinemaContext context, UserManager<User> userManager)
        {
            _db = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Reviews(int id)
        {
            var film = _db.Films.Include(f => f.Genre).FirstOrDefault(f => f.Id == id);
            var reviews = _db.Reviews.Where(r => r.FilmId == id).ToList();
            double TotalRating = 0;
            if (reviews.Count() > 0)
            {
                foreach (var review in reviews)
                    TotalRating += review.Rating;
                TotalRating /= reviews.Count();
            }
            ViewBag.TotalRating = TotalRating;
            ViewBag.Film = film;
            ViewBag.Reviews = reviews; 
            if (User.Identity.IsAuthenticated)
            {
                User user = await _userManager.GetUserAsync(User);
                ViewBag.Blocked = user.Blocked;
                if(!user.Blocked)
                    ViewBag.CheckPurchased = checkFilmsForUser(id).GetAwaiter().GetResult();
            }
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(Review review)
        {
            if (review == null)
                return NotFound();
            User user = await _userManager.GetUserAsync(User);
            review.DatePosted = DateTime.Now;
            review.UserId = user.Id;
            review.UserName = user.UserName;
            var existingReview = _db.Reviews.Where(r => r.UserId == review.UserId).FirstOrDefault(r => r.FilmId == review.FilmId);
            if (existingReview != null)
            {
                existingReview.DatePosted = review.DatePosted;
                existingReview.Text = review.Text;
                existingReview.Rating = review.Rating;
                _db.Entry(existingReview).State = EntityState.Modified;
            }
            else
                _db.Reviews.Add(review);
            _db.SaveChanges();
            return RedirectToAction("Reviews", new {id = review.FilmId });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int? id)
        {
            Review review = _db.Reviews.Include(r => r.Film).FirstOrDefault(review => review.Id == id);
            return View(review);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(Review review)
        {
            if (review != null)
            {
                _db.Entry(review).State = EntityState.Deleted;
                _db.SaveChanges();
            }
            return RedirectToAction("Reviews", new {id = review.FilmId});
        }


        public async Task<bool> checkFilmsForUser(int id)
        {
            User user = await _userManager.GetUserAsync(User);
            List<Order> orders = _db.Orders.Where(o => o.Status == "Выполнен" && o.UserId == user.Id).ToList();
            List<int> PurchasedFilms = new List<int>();
            foreach (var order in orders)
            {
                var items = _db.Items.Where(i => i.OrderId == order.Id).ToList();
                foreach (var item in items)
                {
                    PurchasedFilms.Add(item.FilmId);
                }
            }
            return PurchasedFilms.Contains(id);
        }
    }
}
