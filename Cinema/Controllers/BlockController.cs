using Cinema.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Cinema.Controllers
{
    public class BlockController : Controller
    {
        CinemaContext _db;
        public BlockController(CinemaContext context)
        {
            _db = context;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            List<ReviewBlockList> blocklist = _db.BlockList.ToList();
            return View(blocklist);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Block(int? id)
        {
            Review review = _db.Reviews.Include(r => r.Film).FirstOrDefault(review => review.Id == id);
            return View(review);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Block(Review review)
        {
            ReviewBlockList blockUser = new ReviewBlockList()
            {
                Date = DateTime.Now,
                UserId = review.UserId,
                UserName = review.UserName
            };
            User user = _db.Users.FirstOrDefault(u => u.Id == blockUser.UserId);
            user.Blocked = true;
            _db.Entry(user).State = EntityState.Modified;
            _db.BlockList.Add(blockUser);
            List<Review> reviewsUser = _db.Reviews.Where(r => r.UserId == review.UserId).ToList();
            _db.Reviews.RemoveRange(reviewsUser);
            _db.SaveChanges();
            return RedirectToAction("Index", "Film");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult UnBlock(int? id)
        {
            ReviewBlockList block = _db.BlockList.FirstOrDefault(r => r.Id == id);
            return View(block);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult UnBlock(ReviewBlockList block)
        {
            if (block != null)
            {
                User user = _db.Users.FirstOrDefault(u => u.Id == block.UserId);
                user.Blocked = false;
                _db.Entry(user).State = EntityState.Modified;
                _db.Entry(block).State = EntityState.Deleted;
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
