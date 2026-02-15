using Infrastructure.Seed.Identity;
using Infrastructure.Seed.System;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Seed
{
    public static class ProgramEntry
    {
        public static void Seed(ModelBuilder builder)
        {
            UserRoles.Seed(builder);
            DefaultUser.Seed(builder);
            Data.FakerEntry.Seed(builder);
        }
    }
}
