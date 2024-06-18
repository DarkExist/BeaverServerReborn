using Microsoft.EntityFrameworkCore;

namespace BeaverServerReborn
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Upgrade> Upgrades { get; set; } = null!;
        public DbSet<PlayerUpgrade> PlayerUpgrades { get; set; } = null!;

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Username);
                entity.HasIndex(e => e.Username).IsUnique();
                entity.Property(e => e.Username).IsRequired();
                entity.Property(e => e.Password).IsRequired();
                entity.Property(e => e.Balance).IsRequired();
                entity.Property(e => e.CreationDate).IsRequired();
                entity.Property(e => e.LastEnteredDate).IsRequired();

            });

            modelBuilder.Entity<Upgrade>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();
                entity.HasKey(e => e.Name);
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.AdditionalDescription).IsRequired();
                entity.Property(e => e.Income).IsRequired();
                entity.Property(e => e.Price).IsRequired();
            });

            modelBuilder.Entity<PlayerUpgrade>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd(); // автоинкремент
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Username).IsUnique();
                entity.Property(e => e.Username).IsRequired();
                entity.Property(e => e.UpgradeName).IsRequired();
                entity.Property(e => e.Count).IsRequired();
            });

        }
    }
}
