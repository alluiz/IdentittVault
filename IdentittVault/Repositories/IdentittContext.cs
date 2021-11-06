using IdentittVault.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdentittVault.Repositories
{
    public class IdentittVaultContext : DbContext
    {
        public IdentittVaultContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(p => p.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<Account>()
                    .HasOne(a => a.User)
                    .WithMany(u => u.Accounts)
                    .HasForeignKey(a => a.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Account>()
                .Property(p => p.Id).ValueGeneratedOnAdd();
        }
    }
}
