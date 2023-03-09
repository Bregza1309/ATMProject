using Microsoft.EntityFrameworkCore;
namespace ATM_EFCore
{
    public class ATMdb:DbContext
    {
        public DbSet<Account>? Accounts { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Set Connection String
            optionsBuilder.UseSqlServer(@"Data Source=.;Initial Catalog=ATMDb;Integrated Security=True;Encrypt=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .Property(p => p.LastName)
                .HasMaxLength(40)
                .IsRequired();

            modelBuilder.Entity<Account>()
                .Property(p => p.FirstName)
                .HasMaxLength(40)
                .IsRequired();

            modelBuilder.Entity<Account>()
                .Property(p => p.Pin)
                .IsRequired();
        }
    }
}
