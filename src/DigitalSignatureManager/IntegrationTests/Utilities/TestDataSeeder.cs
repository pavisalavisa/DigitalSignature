using Domain.Common;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;

namespace IntegrationTests.Utilities
{
    public static class TestDataSeeder
    {
        public static void Seed(DigitalSignatureManagerDbContext context)
        {
            SeedRoles(context);
            SeedUsers(context);

            context.SaveChanges();
        }

        private static void SeedRoles(DigitalSignatureManagerDbContext context)
        {
            context.Roles.AddRange(new ApplicationRole
            {
                Name = "Admin",
                NormalizedName = "ADMIN",
            }, new ApplicationRole
            {
                Name = "RegularUser",
                NormalizedName = "REGULARUSER",
            });

            context.SaveChanges();
        }

        private static void SeedUsers(DigitalSignatureManagerDbContext context)
        {
            var admin = new ApplicationUser
            {
                UserName = "kristicevic.antonio@gmail.com",
                NormalizedUserName = "KRISTICEVIC.ANTONIO@GMAIL.COM",
                Email = "kristicevic.antonio@gmail.com",
                NormalizedEmail = "KRISTICEVIC.ANTONIO@GMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAEN7MoQThTdDJuLA9nXofUe+su/DrO1ZvzQ7YrIToZldGO3CsNIG39/ijv7/Tic16Lg==",
                SecurityStamp = "YH4RNOWK5NF5OS52GL5AROXT6B727TYZ"
            };

            var regularUser1 = new ApplicationUser
            {
                UserName = "jon.doe@gmail.com",
                NormalizedUserName = "JON.DOE@GMAIL.COM",
                Email = "jon.doe@gmail.com",
                NormalizedEmail = "JON.DOE@GMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAEN7MoQThTdDJuLA9nXofUe+su/DrO1ZvzQ7YrIToZldGO3CsNIG39/ijv7/Tic16Lg==",
                SecurityStamp = "YH4RNOWK5NF5OS52GL5AROXT6B727TYZ"
            };

            context.Users.AddRange(admin, regularUser1);
            var regularUser2 = new ApplicationUser
            {
                UserName = "jane.doevski@gmail.com",
                NormalizedUserName = "JANE.DOEVSKI@GMAIL.COM",
                Email = "jane.doevski@gmail.com",
                NormalizedEmail = "JANE.DOEVSKI@GMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAEN7MoQThTdDJuLA9nXofUe+su/DrO1ZvzQ7YrIToZldGO3CsNIG39/ijv7/Tic16Lg==",
                SecurityStamp = "YH4RNOWK5NF5OS52GL5AROXT6B727TYZ"
            };

            context.Users.AddRange(admin, regularUser2);
            context.SaveChanges();

            var adminRoles = new IdentityUserRole<int>
            {
                RoleId = 1,
                UserId = admin.Id
            };

            var regularUserRoles1 = new IdentityUserRole<int>
            {
                RoleId = 2,
                UserId = regularUser1.Id
            };
            
            context.UserRoles.AddRange(adminRoles, regularUserRoles1);
            
            var regularUserRoles2 = new IdentityUserRole<int>
            {
                RoleId = 2,
                UserId = regularUser2.Id
            };
            
            context.UserRoles.AddRange(adminRoles, regularUserRoles2);
            context.SaveChanges();
        }
    }
}