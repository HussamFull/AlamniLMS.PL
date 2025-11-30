using AlamniLMS.BLL.Services.Classes;
using AlamniLMS.BLL.Services.Interfacese;
using AlamniLMS.DAL.Repository.Classes;
using AlamniLMS.DAL.Repository.Interfaces;
using AlamniLMS.DAL.Utils;
using AlamniLMS.PL.utils;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace AlamniLMS.PL
{
    internal static class AppConfiguration
    {
        internal static void Config(this IServiceCollection services)
        {
            // Configuration logic goes here
            // Dependency Injection for Categories
            services.AddScoped<ICategoriesService, CategoriesService>();
            services.AddScoped<ICategoriesRepository, CategoriesRepository>();
           
           // servicesjection for Brand Repository
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IBrandService, BrandService>();
          
           // servicesjection for Course Repository
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<ICourseService, CourseService>();
            
           // servicesjection for Lecture Repository
            services.AddScoped<ILectureRepository, LectureRepository>();
            services.AddScoped<ILectureService, LectureService>();
            
            //servicesjection for Enrollment Repository
            services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
            services.AddScoped<IEnrollmentService, EnrollmentService>();
            
            //servicesjection for CheckOut Service
            services.AddScoped<ICheckOutService, CheckOutService>();
            
            //servicesjection for Order Repository
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderService, OrderService>();
            
            //servicesjection for Order Item Repository
            services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            
            //servicesjection for User Service
            services.AddScoped<IUserService, UserService>();
            //servicesjection for User Repository
            services.AddScoped<IUserRepository, UserRepository>();
            
            //servicesjection for Certificate Repository
            services.AddScoped<ICertificateService, CertificateService>();
            services.AddScoped<ICertificateRepository, CertificateRepository>();
           
            //servicesjection for Review Repository
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IReviewService, BLL.Services.Classes.ReviewService>();
           
           // servicesjection for File Service
            services.AddScoped<IFileService, BLL.Services.Classes.FileService>();
            
            
            //servicesjection for Seed Data
            services.AddScoped<ISeedData, SeedData>();
            
            //servicesjection for Authentication Service
            services.AddScoped<IAuthenticationService, AuthenticationSerive>();
            
            //servicesjection for Email Sender
            services.AddScoped<IEmailSender, EmailSetting>();



        }
    }
}
