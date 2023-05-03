
using Core.Abstractions.Repositories;
using Core.Abstractions.Services;
using DatabaseEF.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Models.Validators;
using Models.ViewModels;
using Services;
using System.Configuration;
using WebShop.Authorization.Constants;
using WebShop.Authorization.Handlers;
using WebShop.Authorization.Requirements;
using WebShop.DatabaseEF.Entities;
using WebShop.Middleware;

namespace WebShop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}.json", optional: true)
                .AddJsonFile("appsettings.my.json", optional: true, reloadOnChange: true);


            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContextPool<WebshopContext>(x => 
            {
                x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSingleton<IAuthorizationHandler, RequireAdminHandler>();
            builder.Services.AddAuthorization(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .AddRequirements(new AdminRoleRequirement())
                    .Build();

                options.AddPolicy(AuthorizationPolicies.RequireAdminPolicy, policy);
            });

            builder.Services.AddTransient<IProductsService, ProductsService>();
            builder.Services.AddTransient<ICategoryService, CategoryService>();

            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

            builder.Services.AddTransient<IUsersService, UsersService>();
            builder.Services.AddSingleton<IUserRepository, UserRepository>();

            builder.Services.AddSingleton<IValidator<ProductViewModel>, ProductViewModelValidator>();
            builder.Services.AddSingleton<IValidator<CategoryViewModel>, CategoryViewModelValidator>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseSession();

            app.UseAuthorization();
            
            app.UseCors(x => x  
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
               
            app.MapControllers();

            app.Run();
        }
    }
}