using BankingSystem.ApplicationLogic.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BankingSystem.EFDataAccess
{
    public class BankingSystemDbContext : DbContext
    {
        public BankingSystemDbContext(DbContextOptions<BankingSystemDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserBankAccounts> UserBankAccounts { get; set; }
        public DbSet<UserTransaction> UserTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasKey(t => t.UserId);

            modelBuilder.Entity<UserBankAccounts>()
               .HasKey(t => t.AccountId);

            modelBuilder.Entity<UserTransaction>()
               .HasKey(t => t.TransactionId);

            modelBuilder.Entity<UserBankAccounts>()
                .HasOne(t => t.User)
                .WithMany(t => t.UserBankAccounts)
                .HasForeignKey(f => f.UserId); 

            modelBuilder.Entity<UserTransaction>()
                .HasOne(a => a.FromAccount)
                .WithMany(t => t.FromTransactions)
                .HasForeignKey(f => f.FromAccountId);

            modelBuilder.Entity<UserTransaction>()
                .HasOne(a => a.ToAccount)
                .WithMany(t => t.ToTransactions)
                .HasForeignKey(f => f.ToAccountId);
                
            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                  foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

        }
    }
}
