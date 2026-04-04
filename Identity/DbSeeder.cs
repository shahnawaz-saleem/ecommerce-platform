using Microsoft.AspNetCore.Identity;

namespace Identity
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            var email = "admin@test.com";
            var password = "Admin@123";

            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,FullName = "admincer"
                };

                await userManager.CreateAsync(user, password);
            }
        }
    }
}
