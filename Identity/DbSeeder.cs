using Microsoft.AspNetCore.Identity;

namespace Identity
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(
     UserManager<ApplicationUser> userManager,
     RoleManager<IdentityRole> roleManager)
        {
            // Roles
            string[] roles = { "Admin", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // User
            var email = "admin@test.com";
            var password = "Admin@123";

            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email
                };

                await userManager.CreateAsync(user, password);
            }

            // Assign role
            if (!await userManager.IsInRoleAsync(user, "Admin"))
            {
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}
