
using Core.Abstractions.Repositories;
using Core.Abstractions.Services;
using DatabaseEF.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models.Validators;
using Models.ViewModels;
using Services;
using WebShop.DatabaseEF.Entities;

namespace WebShop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContextPool<WebshopContext>(x => 
            {
                x.UseSqlServer("Server=CTSE-GORANZ;Database=Webshop;Trusted_Connection=True;");
            });

            builder.Services.AddTransient<IProductsService, ProductsService>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

            builder.Services.AddTransient<IUsersService, UsersService>();
            builder.Services.AddSingleton<IUserRepository, UserRepository>();

            builder.Services.AddSingleton<IValidator<ProductViewModel>, ProductViewModelValidator>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.UseSession();

            app.MapControllers();

            app.Run();
        }
    }
}