using Domain.Constants;
using Domain.Entities;
using Domain.Entities.Identity;
using Domain.Enums;
using Infrastructure.Seed.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Seed.System
{
    internal static class DefaultUser
    {
        public static void Seed(ModelBuilder builder)
        {
            // Define the default admin user
            var adminUser = new Administrator
            {
                Id = Guid.NewGuid(),
                FirstName = "Siyavuya",
                LastName = "Chagi",
                Gender = Gender.Male,
                UserName = "5001015009087",
                NormalizedUserName = "5001015009087",
                Email = "syavuya08@outlook.com",
                NormalizedEmail = "SYAVUYA08@OUTLOOK.COM",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
            };

            // Hash the password
            var passwordHasher = new PasswordHasher<Administrator>();
            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "Admin@123"); // Change password in prod

            // Seed user
            builder.Entity<Administrator>().HasData(adminUser);

            // Seed user role assignment
            builder.Entity<UserRoleLink>().HasData(new UserRoleLink
            {
                RoleId = DeterministicGuid.Create(Roles.Admin),      // Admin role ID from UserRoles seeder
                UserId = adminUser.Id
            });
        }
    }
}

