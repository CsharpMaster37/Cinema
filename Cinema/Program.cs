using Cinema.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string connString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CinemaContext>(options =>
                                    options.UseSqlServer(connString));// Add services to the container.
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<CinemaContext>()
    .AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(options =>
{
    // Минимальная длина пароля
    options.Password.RequiredLength = 6;
    // Время блокировки пользователя после неудачных попыток входа в систему
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    // Количество неудачных попыток входа в систему, после которых пользователь будет заблокирован
    options.Lockout.MaxFailedAccessAttempts = 5;
    // Устанавливаем требование к наличию не алфавитно-цифровых символов в пароле на false
    options.Password.RequireNonAlphanumeric = false;
    // Устанавливаем требование к наличию строчной буквы ('a'-'z') в пароле на false
    options.Password.RequireLowercase = false;
    // Устанавливаем требование к наличию прописной буквы ('A'-'Z') в пароле на false
    options.Password.RequireUppercase = false;
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Админ"));
});
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Film}/{action=Index}/{id?}");

app.Run();
