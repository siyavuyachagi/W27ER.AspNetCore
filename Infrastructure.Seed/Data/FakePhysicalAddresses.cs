using Bogus;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Seed.Data
{
    internal class FakePhysicalAddresses
    {
        public static void Seed(ModelBuilder builder)
        {
            var faker = new Faker<PhysicalAddress>("en_ZA")
                .RuleFor(a => a.Id, _ => Guid.NewGuid())
                .RuleFor(a => a.StreetNumber, f =>
                    f.Random.Bool(0.8f)
                        ? f.Random.Number(1, 9999).ToString()
                        : null)
                .RuleFor(a => a.StreetName, f => $"{f.Name.LastName()} Street")
                .RuleFor(a => a.Suburb, f => f.Address.County())
                .RuleFor(a => a.City, f => f.Address.City())
                .RuleFor(a => a.Province, f => "Eastern Cape")
                .RuleFor(a => a.PostalCode, f => f.Random.Number(1000, 9999).ToString())
                .RuleFor(a => a.Country, _ => "South Africa")
                .RuleFor(a => a.AdditionalDetails, f =>
                    f.Random.Bool(0.3f) ? $"Flat {f.Random.Number(1, 20)}" : null)
                .RuleFor(a => a.Latitude, f => f.Address.Latitude(-35, -22))
                .RuleFor(a => a.Longitude, f => f.Address.Longitude(16, 33))
                .RuleFor(a => a.CreatedAt, _ => DateTime.UtcNow)
                .RuleFor(a => a.UpdatedAt, _ => null)
                .RuleFor(a => a.DeletedAt, _ => null)
                .RuleFor(a => a.IsDeleted, _ => false);

            var addresses = faker.Generate(5);

            FakerEntry.PhysicalAddresses.AddRange(addresses);
            builder.Entity<PhysicalAddress>().HasData(addresses);
        }
    }
}

