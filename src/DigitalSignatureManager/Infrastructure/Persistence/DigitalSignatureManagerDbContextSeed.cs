using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Contracts;
using Domain.Enums;

namespace Infrastructure.Persistence
{
    public static class DigitalSignatureManagerDbContextSeed
    {
        public static async Task SeedSampleDataAsync(DigitalSignatureManagerDbContext context,
            IApplicationRoleManager roleManager, IApplicationUserManager userManager)
        {
            // Seed, if necessary
            await SeedRoles(context, roleManager);
            await SeedUsers(context, userManager);

            await context.SaveChangesAsync();
        }

        private static async Task SeedRoles(DigitalSignatureManagerDbContext context,
            IApplicationRoleManager roleManager)
        {
            var allRoles = Enum.GetNames(typeof(Roles));
            var existingRoles = context.Roles.Select(r => r.Name).ToList();

            var missingRoles = allRoles.Except(existingRoles);

            foreach (var role in missingRoles)
            {
                await roleManager.CreateRoleAsync(Enum.Parse<Roles>(role));
            }
        }

        private static async Task SeedUsers(DigitalSignatureManagerDbContext context,
            IApplicationUserManager userManager)
        {
            const string seedAdminUsername = "kristicevic.antonio@gmail.com";

            if (!context.Users.Any(usr => usr.UserName == seedAdminUsername))
            {
                var user = await userManager.CreateUser(seedAdminUsername, "Admin.123");
                await userManager.AddToRole(user.Id, Roles.Admin);
            }
        }
    }
}