using Microsoft.EntityFrameworkCore;
using UserRegistrationApi.Models;

namespace UserRegistrationApi.Data
{
    public class UserRegistrationContext : DbContext
    {
        public UserRegistrationContext(DbContextOptions<UserRegistrationContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        // Add other DbSet properties for your models as needed
    }
}
