using Domain.Constants;
using Domain.Entities.Identity;
using Infrastructure.Seed.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Seed.Identity
{
    internal static class UserRoles
    {
        public static void Seed(ModelBuilder builder)
        {
            builder.Entity<Role>().HasData(
                new Role
                {
                    Id = DeterministicGuid.Create(Roles.Admin),
                    Name = Roles.Admin,
                    NormalizedName = Roles.Admin.ToUpper()
                },
                new Role
                {
                    Id = DeterministicGuid.Create(Roles.User),
                    Name = Roles.User,
                    NormalizedName = Roles.User.ToUpper()
                }
            );
        }
    }
}
