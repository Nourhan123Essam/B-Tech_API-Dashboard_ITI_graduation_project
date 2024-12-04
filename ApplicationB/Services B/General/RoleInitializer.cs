using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ModelsB.Authentication_and_Authorization_B;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationB.Services_B.General
{
    public class RoleInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUserB>>();

            string[] roleNames = { "Admin", "Customer" };
            IdentityResult roleResult;

            // Create roles if they don't exist
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    // Create the roles and seed them to the database
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Create a default Admin user if it doesn't exist
            var adminEmail = "moh.alnoby216@gmail.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUserB
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    UserType = "Admin" // Assign UserType
                };
                await userManager.CreateAsync(adminUser, "BTECH@g5"); // Change the password as needed
            }

            // Assign the admin user to the Admin role
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}
