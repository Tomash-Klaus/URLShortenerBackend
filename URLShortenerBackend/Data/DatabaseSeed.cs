using Microsoft.AspNetCore.Identity;
using System.Data;

namespace URLShortenerBackend.Data
{
    public static class DatabaseSeed
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            await SeedRolesAsync(roleManager);
            await SeedUsersAsync(userManager);
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Admin", "User" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        private static async Task SeedUsersAsync(UserManager<IdentityUser> userManager)
        {
            var adminEmail = "admin@mailforspam.com";
            var userEmail = "user@mailforspam.com";

            var users = new List<Tuple<IdentityUser, string>>()
            {
                Tuple.Create(
                    new IdentityUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        EmailConfirmed = true,
                        PhoneNumber = "+380999999999"
                    },
                    "Admin"
                   ),

                Tuple.Create(
                    new IdentityUser
                    {
                        UserName = userEmail,
                        Email = userEmail,
                        EmailConfirmed = true,
                        PhoneNumber = "+380999999999"
                    },
                    "User"
                )

            };

            await AddToDb(userManager, users);
        }


        private static async Task AddToDb(UserManager<IdentityUser> userManager, List<Tuple<IdentityUser, string>> users)
        {
            foreach (var user in users)
            {
                if (await userManager.FindByEmailAsync(user.Item1.Email!) == null)
                {
                    var result = await userManager.CreateAsync(user.Item1, "Password99");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user.Item1, user.Item2);
                    }
                }
            }
        }
    }
}
