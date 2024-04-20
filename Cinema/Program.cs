using Cinema.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();
//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
//    if (!await roleManager.RoleExistsAsync("Admin"))
//    {
//        var role = new IdentityRole("Admin");
//        await roleManager.CreateAsync(role);
//    }
//}

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
