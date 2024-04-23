using Cinema.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Controllers
{
    public class OrderController : Controller
    {
        CinemaContext _db;
        UserManager<User> _userManager;
        public OrderController(CinemaContext context, UserManager<User> userManager)
        {
            _db = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            User user = await _userManager.GetUserAsync(User);
            Order order = await _db.Orders.FirstOrDefaultAsync(c => c.UserId == user.Id);
            if (order != null && order.Status != "Выполнен")
            {
                List<OrderItem> orderItem = _db.Items.Where(c => c.OrderId == order.Id).ToList();
                _db.Items.RemoveRange(orderItem);
                _db.Orders.Remove(order);
            }
            order = new Order()
            {
                TotalPrice = 0,
                Date = DateTime.Now,
                UserId = user.Id
            };
            var orderItems = new List<OrderItem>();
            List<CartItem> cartList = _db.Cart.Where(c => c.UserId == user.Id).ToList();
            int Sum = 0;
            foreach (var cart in cartList)
            {
                var film = _db.Films.Find(cart.FilmId);
                if (film == null)
                {
                    continue;
                }
                var orderItem = new OrderItem()
                {
                    FilmId = film.Id,
                    Film = film
                };
                orderItems.Add(orderItem);
                Sum += film.Price;
                _db.Entry(film).State = EntityState.Modified;
            }
            if (orderItems.Count == 0)
                return RedirectToAction("Index", "Cart");
            order.Items = orderItems;
            order.TotalPrice = Sum;
            order.Status = "Создан";
            _db.Orders.Add(order);
            _db.Items.AddRange(orderItems);
            _db.SaveChanges();
            return View(order);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(Order order)
        {
            order.Status = "Выполнен";
            _db.Entry(order).State = EntityState.Modified;
            var carts = _db.Cart.Where(c => c.UserId == order.UserId).ToList();
            _db.RemoveRange(carts);
            _db.SaveChanges();
            return RedirectToAction("Success");
        }
        [Authorize]
        public IActionResult Success()
        {
            return View();
        }
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            User user = await _userManager.GetUserAsync(User);
            Order order = await _db.Orders.FirstOrDefaultAsync(c => c.UserId == user.Id);
            List<OrderItem> orderItem = _db.Items.Where(c => c.OrderId == order.Id).ToList();
            _db.Items.RemoveRange(orderItem);
            _db.Orders.Remove(order);
            _db.SaveChanges();
            return RedirectToAction("Index", "Film");
        }
    }
}
