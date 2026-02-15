using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Seed.Data
{
    public class FakerEntry
    {
        public static List<PhysicalAddress> PhysicalAddresses { get; set; } = [];
        public static void Seed(ModelBuilder builder)
        {
            FakePhysicalAddresses.Seed(builder);
            FakeUsers.Seed(builder);
        }
    }
}
