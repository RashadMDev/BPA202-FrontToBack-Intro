using Microsoft.EntityFrameworkCore;
using ProniaFrontToBack.DAL;

namespace ProniaFrontToBack;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllersWithViews();
        builder.Services.AddDbContext<AppDbContext>(options =>
                       {
                           options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                       });
        var app = builder.Build();
        app.UseStaticFiles();


        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}"
        );

        app.Run();
    }
}
