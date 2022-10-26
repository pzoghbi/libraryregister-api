
using LibraryRegister.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MySql.EntityFrameworkCore.Extensions;
internal class Program
{
    public static void Main(string[] args)
    {
        // Services
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddCors(setupAction: options => { 
            options.AddDefaultPolicy( policy => { 
                policy.WithOrigins(
                    "http://localhost:3000", 
                    "https://localhost:3000",
                    "http://localhost:3000/*",
                    "https://localhost:3000/*"
                )
                .AllowAnyHeader();
            });
        });
        builder.Services.AddControllers();
        builder.Services.AddDbContext<LibraryDbContext>();

        // Middleware
        var app = builder.Build();
        app.UseHttpsRedirection();
        app.UseCors();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}

