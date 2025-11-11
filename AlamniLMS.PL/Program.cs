
using AlamniLMS.BLL.Services.Classes;
using AlamniLMS.BLL.Services.Interfacese;

using AlamniLMS.DAL.Data;
using AlamniLMS.DAL.Models;
using AlamniLMS.DAL.Repository.Classes;
using AlamniLMS.DAL.Repository.Interfaces;
using AlamniLMS.DAL.Utils;
using AlamniLMS.PL.utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar;
using Scalar.AspNetCore;
using System.Text;

namespace AlamniLMS.PL
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddControllers();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Dependency Injection for Categories
            builder.Services.AddScoped<ICategoriesRepository,CategoriesRepository>();
            builder.Services.AddScoped<ICategoriesService,CategoriesService>();

            // Dependency Injection for Brand Repository
            builder.Services.AddScoped<IBrandRepository, BrandRepository>();
            builder.Services.AddScoped<IBrandService, BrandService>();

            // Dependency Injection for Course Repository
            builder.Services.AddScoped<ICourseRepository, CourseRepository>();
            builder.Services.AddScoped<ICourseService, CourseService>();

            // Dependency Injection for File Service
            builder.Services.AddScoped<IFileService, FileService>();


            // Dependency Injection for Seed Data
            builder.Services.AddScoped<ISeedData, SeedData>();

            // Dependency Injection for Authentication Service
            builder.Services.AddScoped<IAuthenticationService, AuthenticationSerive>();

            // Dependency Injection for Email Sender
            builder.Services.AddScoped<IEmailSender, EmailSetting>();





            // Register Identity services
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                 .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders(); 
            //options =>
            //{
            //options.Password.RequireDigit = true;
            //options.Password.RequireLowercase = true;
            //options.Password.RequireUppercase = true;
            //options.Password.RequireNonAlphanumeric = false;
            //options.Password.RequiredLength = 8;
            //options.User.RequireUniqueEmail = true;
            //options.SignIn.RequireConfirmedEmail = true;
            //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
            //options.Lockout.MaxFailedAccessAttempts = 10;
            // options.Lockout.AllowedForNewUsers = true;
            //}

            //)
            //.AddEntityFrameworkStores<ApplicationDbContext>()
            //.AddDefaultTokenProviders();


            // JWT Authentication Configuration
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })

               .AddJwtBearer(options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = false,
                       ValidateAudience = false,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwtOptions:SecretKey"]))
                    };
                });







            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            /// Seed Data
            var scope = app.Services.CreateScope();
            var objectOfSeedData = scope.ServiceProvider.GetRequiredService<ISeedData>();
            await objectOfSeedData.DataSeedingAsync();
            await objectOfSeedData.IdentityDataSeedingAsync();

           app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseStaticFiles();


            app.MapControllers();

            app.Run();
        }
    }
}
