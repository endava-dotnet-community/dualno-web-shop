
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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Domain;
using Microsoft.AspNetCore.Authentication.Cookies;

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
                .AddJsonFile("appsettings.my.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

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

            builder.Services
                .AddIdentity<IdentityUser, IdentityRole>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                })
                .AddEntityFrameworkStores<WebshopContext>();

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IAuthorizationHandler, RequireAdminHandler>();
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
            builder.Services.AddScoped<UserManager<IdentityUser>>();
            builder.Services.AddScoped<SignInManager<IdentityUser>>();
            builder.Services.AddScoped<RoleManager<IdentityRole>>();
            //builder.Services.AddScoped<JwtService>();

            builder.Services.AddSingleton<IValidator<ProductViewModel>, ProductViewModelValidator>();
            builder.Services.AddSingleton<IValidator<CategoryViewModel>, CategoryViewModelValidator>();

            //// add jwt authentication
            //builder.Services
            //    .AddAuthentication(x =>
            //    {
            //        x.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //    })
            //    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            //    .AddCookie(IdentityConstants.ApplicationScheme)
            //    .AddJwtBearer(options =>
            //    {
            //        options.TokenValidationParameters = new TokenValidationParameters()
            //        {
            //            ValidateIssuer = true,
            //            ValidateAudience = true,
            //            ValidateLifetime = true,
            //            ValidateIssuerSigningKey = true,
            //            ValidAudience = builder.Configuration["Jwt:Audience"],
            //            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            //            IssuerSigningKey = new SymmetricSecurityKey(
            //                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
            //            )
            //        };
            //    });

            builder.Services.AddSwaggerGen(c =>
            {
                //c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                //{
                //    Name = "Authorization",
                //    Type = SecuritySchemeType.ApiKey,
                //    Scheme = "Bearer",
                //    BearerFormat = "JWT",
                //    In = ParameterLocation.Header,
                //    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                //});
                //c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                //    {
                //        new OpenApiSecurityScheme {
                //            Reference = new OpenApiReference {
                //                Type = ReferenceType.SecurityScheme,
                //                    Id = "Bearer"
                //            }
                //        },
                //        new string[] {}
                //    }
                //});
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }



            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseSession();

            _ = CreateRolesAndUsers(app.Services.CreateScope());

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseCors(x => x
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());

            app.MapControllers();

            app.Run();
        }
        private async static Task CreateRolesAndUsers(IServiceScope scope)
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (!roleManager.Roles.Any())
            {
                foreach (UserRole userRole in Enum.GetValues(typeof(UserRole)))
                {
                    if (!roleManager.Roles.Any(r => r.Name.Equals(userRole.ToString())))
                    {
                        if (!await roleManager.RoleExistsAsync(userRole.ToString()))
                            await roleManager.CreateAsync(new IdentityRole(userRole.ToString()));
                    }
                }
            }

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            if (userManager.Users.FirstOrDefault(u => u.UserName.Equals("Admin")) == null)
            {
                var user = new IdentityUser();
                user.UserName = "Admin";
                user.Email = "admin@endava.com";

                string userPWD = "admin123";

                var chkUser = userManager.CreateAsync(user, userPWD);

                if (chkUser.Result.Succeeded)
                {
                    var result1 = userManager.AddToRoleAsync(user, UserRole.Administrator.ToString());
                }
            }
        }

    }
}