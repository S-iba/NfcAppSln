using Microsoft.EntityFrameworkCore;
using SimpleAPIs.Models;


namespace SimpleAPIs.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public void SeedData()
        {
            if (Users.Any())
            {
                return; // DB has been seeded
            }

            var users = new List<User>
            {
                new User { Username = "Siba-Bus", UUID = "A0-97-3E-CB" },
                new User { Username = "Siba", UUID = "C9-6D-3B-9C" },
                new User { Username = "T-Bus", UUID = "44-CF-88-89" },
                new User { Username = "Clicks-Mars", UUID = "08-06-1B-C9" },
                new User { Username = "Ish", UUID = "C9-05-58-71" },
                new User { Username = "Vuyo", UUID = "46-8C-33-22" },
                new User { Username = "Sharon", UUID = "B9-5C-28-9C" },
                new User { Username = "Tuba", UUID = "39-90-34-9C" },
                new User { Username = "Tee", UUID = "89-34-38-9C" }
            };
            Users.AddRange(users);
            SaveChanges();
        }

    }
}
