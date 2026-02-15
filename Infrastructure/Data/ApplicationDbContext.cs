using Domain.Entities;
using Domain.Entities.Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, Role, Guid>
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            IHttpContextAccessor contextAccessor)
            : base(options)
        {
            _contextAccessor = contextAccessor;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.AddInterceptors(new DatabaseCreateInterceptor());
            //optionsBuilder.AddInterceptors(new DatabaseUpdateInterceptor());
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(entity =>
            {
                entity.HasDiscriminator<string>("Discriminator")
                .HasValue<Administrator>(nameof(Administrator))
                .HasValue<User>(nameof(User));
            });


            builder.Entity<JobApplication>(entity =>
            {
                entity.HasOne(x => x.Job)
                .WithMany(x => x.JobApplications)
                .HasForeignKey(x => x.JobId)
                .OnDelete(DeleteBehavior.NoAction);
            });


            builder.Entity<JobAssignment>(entity =>
            {
                entity.HasOne(x => x.Job)
                .WithMany(x => x.JobAssignments)
                .HasForeignKey(x => x.JobId)
                .OnDelete(DeleteBehavior.NoAction);
            });


            //builder.Entity<UserRoleLink>(entity =>
            //{
            //    entity.HasOne(x => x.User)
            //});

            Seed.ProgramEntry.Seed(builder);
        }

        #region Database Tables
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<JobAssignment> JobAssignments { get; set; }
        public DbSet<PhysicalAddress> PhysicalAddresses { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Video> Videos { get; set; }
        #endregion
    }
}