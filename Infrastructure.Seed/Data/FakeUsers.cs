using Bogus;
using Domain.Constants;
using Domain.Entities;
using Domain.Entities.Identity;
using Domain.Enums;
using Infrastructure.Seed.Data;
using Infrastructure.Seed.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

internal class FakeUsers
{
    public static void Seed(ModelBuilder builder)
    {
        var passwordHasher = new PasswordHasher<User>();

        var faker = new Faker<User>("en_ZA")
            .RuleFor(u => u.Id, _ => Guid.NewGuid())
            .RuleFor(u => u.FirstName, f => f.Name.FirstName())
            .RuleFor(u => u.MiddleName, f => f.Random.Bool(0.5f) ? f.Name.FirstName() : null)
            .RuleFor(u => u.LastName, f => f.Name.LastName())
            .RuleFor(u => u.Gender, f => f.PickRandom<Gender>())
            .RuleFor(u => u.PhysicalAddressId, f => f.PickRandom(FakerEntry.PhysicalAddresses.Select(pa => pa.Id)))
            .RuleFor(u => u.ProfileImageUrl, f => f.Internet.Avatar())
            .RuleFor(u => u.UserName, (f, u) => GenerateIdNumber(u.Gender))
            .RuleFor(u => u.NormalizedUserName, (f, u) => u.UserName.ToUpper())
            .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
            .RuleFor(u => u.NormalizedEmail, (f, u) => u.Email.ToUpper())
            .RuleFor(u => u.EmailConfirmed, _ => true)
            .RuleFor(u => u.SecurityStamp, _ => Guid.NewGuid().ToString("D"))
            .RuleFor(u => u.ConcurrencyStamp, _ => Guid.NewGuid().ToString());

        var users = faker.Generate(10);

        // Hash passwords AFTER generation
        foreach (var user in users)
        {
            user.PasswordHash = passwordHasher.HashPassword(user, "Pass@123");
            // Seed user role assignment
            builder.Entity<UserRoleLink>().HasData(new UserRoleLink
            {
                RoleId = DeterministicGuid.Create(Roles.User),      // User role ID from UserRoles seeder
                UserId = user.Id
            });
        }

        builder.Entity<User>().HasData(users);
    }


    public static string GenerateIdNumber(Gender gender)
    {
        var random = new Random();

        // Date of birth (18–60 years old)
        var minDate = DateTime.UtcNow.AddYears(-60);
        var maxDate = DateTime.UtcNow.AddYears(-18);
        var dob = minDate.AddDays(random.Next((maxDate - minDate).Days));
        string dobPart = dob.ToString("yyMMdd");

        // Gender-based sequence
        int sequence = gender switch
        {
            Gender.Male => random.Next(5000, 10000),
            Gender.Female => random.Next(0, 5000),
            _ => random.Next(0, 10000)
        };
        string sequencePart = sequence.ToString("D4");

        int citizenship = 0;
        int usuallyEight = 8;

        string first12 = $"{dobPart}{sequencePart}{citizenship}{usuallyEight}";
        int checksum = CalculateLuhnDigit(first12);

        return first12 + checksum;
    }


    private static int CalculateLuhnDigit(string number)
    {
        int sum = 0;
        bool alternate = true;

        for (int i = number.Length - 1; i >= 0; i--)
        {
            int n = int.Parse(number[i].ToString());

            if (alternate)
            {
                n *= 2;
                if (n > 9)
                    n -= 9;
            }

            sum += n;
            alternate = !alternate;
        }

        return (10 - (sum % 10)) % 10;
    }

}
