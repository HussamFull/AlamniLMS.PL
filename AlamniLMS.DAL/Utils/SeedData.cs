using AlamniLMS.DAL.Data;
using AlamniLMS.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlamniLMS.DAL.Utils
{
    public class SeedData : ISeedData
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;


        public SeedData(
            ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager

            )
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task DataSeedingAsync()
        {
            if ((await _context.Database.GetPendingMigrationsAsync()).Any())
            {
                await _context.Database.MigrateAsync();
            }

            if (!await _context.Categories.AnyAsync())
            {
                await _context.Categories.AddRangeAsync(
                    new Categories { Name = "Electronics", Description = "Default Description for Electronics" },
                    new Categories { Name = "Books", Description = "Default Description for Books" },
                    new Categories { Name = "Clothing", Description = "Default Description for Clothing" }
    );
            }
            if (!await _context.Brands.AnyAsync())
            {
                await _context.Brands.AddRangeAsync(
                   new Brand { Name = "Samsung" },
                   new Brand { Name = "Apple" },
                   new Brand { Name = "MBrandC" }
               );
                //await _context.Brands.AddRangeAsync(
                //    new Brand { Name = "Samsung", MainImage = "sam.png" },
                //    new Brand { Name = "Apple", MainImage = "Apple.png" },
                //    new Brand { Name = "MBrandC", MainImage = "m.png" }
                //);
            }
            await _context.SaveChangesAsync();
        }

        public async Task IdentityDataSeedingAsync()
        {
            if (!await _roleManager.Roles.AnyAsync())
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                await _roleManager.CreateAsync(new IdentityRole("Customer"));
                // await _roleManager.CreateAsync(new IdentityRole("Perent"));
            }


            if (!await _userManager.Users.AnyAsync())
            {
                var user1 = new ApplicationUser()

                {

                    Email = "hosam813@gmail.com",
                    UserName = "hosam813",
                    FullName = "Hosam Alnabelsi",
                    PhoneNumber = "1234567890",
                    EmailConfirmed = true,

                };
                var user2 = new ApplicationUser()

                {

                    Email = "hamada813@gmail.com",
                    UserName = "hamada",
                    FullName = "hamada Alnabelsi",
                    PhoneNumber = "1234567830",
                    EmailConfirmed = true,

                };
                var user3 = new ApplicationUser()

                {

                    Email = "mohammad@gmail.com",
                    UserName = "mohammad",
                    FullName = "mohammad Alnabelsi",
                    PhoneNumber = "1234567840",
                    EmailConfirmed = true,

                };

                await _userManager.CreateAsync(user1, "Pass@1212");
                await _userManager.CreateAsync(user2, "Pass@1212");
                await _userManager.CreateAsync(user3, "Pass@1212");

                await _userManager.AddToRoleAsync(user1, "Admin");
                await _userManager.AddToRoleAsync(user2, "SuperAdmin");
                await _userManager.AddToRoleAsync(user3, "Customer");


            }

            await _context.SaveChangesAsync();

        }


    }
}
