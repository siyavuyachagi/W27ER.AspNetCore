using Infrastructure.Seed.Identity;
using Infrastructure.Seed.System;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Seed
{
    internal static class Entry
    {
        public static void Seed(ModelBuilder builder)
        {
            UserRoles.Seed(builder);
            DefaultUser.Seed(builder);
        }
    }
}
