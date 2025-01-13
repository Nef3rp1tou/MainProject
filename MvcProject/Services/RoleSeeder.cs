using Microsoft.AspNetCore.Identity;

namespace MvcProject.Services
{
    public class RoleSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            var roles = new[] { "Admin", "Player" };
            foreach (var role in roles) 
            { 
                if(!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            string email = "admin@admin.com";
            string password = "Admin@123";

            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new IdentityUser();
                user.UserName = email;
                user.Email = email;
                user.EmailConfirmed = true;
                await userManager.CreateAsync(user, password);

                await userManager.AddToRoleAsync(user, "Admin");

            }

        }
    }
}
