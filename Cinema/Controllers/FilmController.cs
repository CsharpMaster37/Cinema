using Cinema.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Cinema.Controllers
{
    public class FilmController : Controller
    {
        CinemaContext _db;
        IWebHostEnvironment _environment;
        private readonly UserManager<User> _userManager;
        const int ImageWidth = 368;
        const int ImageHeight = 500;
        public FilmController(CinemaContext db, IWebHostEnvironment hostEnvironment, UserManager<User> userManager)
        {
            _db = db;
            _environment = hostEnvironment;
            _userManager = userManager;
            //var horrorGenre = new Genre() { Name = "Ужасы" };
            //var sciFiGenre = new Genre() { Name = "Фантастика" };

            //var film1 = new Film
            //{
            //    Name = "Кошмар на улице Вязов",
            //    Country = "США",
            //    Year = 1984,
            //    Price = 299,
            //    Genre = horrorGenre
            //};

            //var film2 = new Film
            //{
            //    Name = "Матрица",
            //    Country = "США",
            //    Year = 1999,
            //    Price = 399,
            //    Genre = sciFiGenre
            //};

            //var film3 = new Film
            //{
            //    Name = "Звездные войны: Эпизод IV - Новая надежда",
            //    Country = "США",
            //    Year = 1977,
            //    Price = 349,
            //    Genre = sciFiGenre
            //};

            //var film4 = new Film
            //{
            //    Name = "Чужой",
            //    Country = "США",
            //    Year = 1979,
            //    Price = 429,
            //    Genre = horrorGenre
            //};

            //var film5 = new Film
            //{
            //    Name = "Привидение",
            //    Country = "США",
            //    Year = 1990,
            //    Price = 299,
            //    Genre = horrorGenre
            //};

            //var film6 = new Film
            //{
            //    Name = "Вспомнить всё",
            //    Country = "США",
            //    Year = 1990,
            //    Price = 399,
            //    Genre = sciFiGenre
            //};
            //_db.Genres.Add(horrorGenre);
            //_db.Genres.Add(sciFiGenre);
            //db.Films.AddRange([film1, film2,film3, film4, film5, film6]);
            //db.SaveChanges();
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.UserManager = _userManager;
            var films = await _db.Films.Include(f => f.Genre).ToListAsync();
            List<int> cart = new List<int>();
            List<int> PurchasedFilms = new List<int>();
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                var cartDB = await _db.Cart.Where(c => c.UserId == user.Id).ToListAsync();
                foreach (var item in cartDB)
                {
                    cart.Add(item.FilmId);
                }
                List<Order> orders = _db.Orders.Where(o => o.Status == "Выполнен" && o.UserId == user.Id).ToList();
                foreach (var order in orders)
                {
                    var items = _db.Items.Where(i => i.OrderId == order.Id).ToList();
                    foreach (var item in items)
                    {
                        PurchasedFilms.Add(item.FilmId);
                    }
                }
            }
            ViewBag.Cart = cart;
            ViewBag.PurchasedFilms = PurchasedFilms;
            return View(films);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();
            var film = _db.Films.Include(f => f.Genre).FirstOrDefault(f => f.Id == id);
            if (film == null)
                return NotFound();
            ViewBag.Genre = new SelectList(_db.Genres, "Id", "Name");
            return View(film);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Film film, IFormFile upload)
        {
            if (upload != null)
            {
                string fileName = Path.GetFileName(upload.FileName);
                var extFile = fileName.Substring(fileName.LastIndexOf('.'));
                if (extFile.Contains("png") || extFile.Contains("bmp") || extFile.Contains("jpg")
                    || extFile.Contains("jpeg"))
                {
                    var image = Image.Load(upload.OpenReadStream());
                    image.Mutate(x => x.Resize(ImageWidth, ImageHeight));
                    string path = "\\wwwroot\\images\\" + fileName;
                    var hostPath = _environment.ContentRootPath + path;
                    image.Save(hostPath);
                    film.ImageUrl = fileName;
                }
            }

            _db.Entry(film).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Search(string searchString, int? year)
        {
            ViewBag.UserManager = _userManager;
            ViewBag.SearchString = searchString;
            ViewBag.SelectedYear = year;
            List<Film> films = searchString is not null && searchString.Length > 0 ?
                _db.Films.Include(f => f.Genre).Where(f => f.Name.Contains(searchString)).ToList() :
                _db.Films.Include(f => f.Genre).ToList();
            if (year.HasValue)
            {
                films = films.Where(f => f.Year == year).ToList();
            }
            List<int> cart = new List<int>();
            List<int> PurchasedFilms = new List<int>();
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);

                var cartDB = _db.Cart.Where(c => c.UserId == user.Id).ToList();

                foreach (var item in cartDB)
                {
                    cart.Add(item.FilmId);
                }
                List<Order> orders = _db.Orders.Where(o => o.Status == "Выполнен" && o.UserId == user.Id).ToList();
                foreach (var order in orders)
                {
                    var items = _db.Items
                        .Where(i => i.OrderId == order.Id)
                        .ToList();

                    foreach (var item in items)
                    {
                        PurchasedFilms.Add(item.FilmId);
                    }
                }
            }
            ViewBag.Cart = cart;
            ViewBag.PurchasedFilms = PurchasedFilms;
            return View("Index", films);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.Genre = new SelectList(_db.Genres, "Id", "Name");
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(Film film, IFormFile upload)
        {
            if (upload != null)
            {
                string fileName = Path.GetFileName(upload.FileName);
                var extFile = fileName.Substring(fileName.LastIndexOf("."));
                if (extFile.Contains("png") || extFile.Contains("bmp") || extFile.Contains("jpg") || extFile.Contains("jpeg"))
                {
                    var image = Image.Load(upload.OpenReadStream());
                    image.Mutate(x => x.Resize(ImageWidth, ImageHeight));
                    string path = "\\wwwroot\\images\\" + fileName;
                    var hostPath = _environment.ContentRootPath + path;
                    image.Save(hostPath);
                    film.ImageUrl = fileName;
                }
            }
            _db.Films.Add(film);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();
            Film film = _db.Films.Include(f => f.Genre).FirstOrDefault(f => f.Id == id);
            if (film == null)
                return NotFound();
            return View(film);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(Film film)
        {
            if (film != null)
            {
                _db.Entry(film).State = EntityState.Deleted;
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
