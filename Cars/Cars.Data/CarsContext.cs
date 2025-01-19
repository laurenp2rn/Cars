using Cars.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Cars.Data
{
    public class CarsContext : DbContext
    {
        public CarsContext(DbContextOptions<CarsContext> options)
            : base(options) { }

        public DbSet<Car> Cars { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Car>()
                .ToTable("Cars");

            modelBuilder.Entity<Car>()
                .HasKey(c => c.Id);
        }

        // Muuda migratsioonide kogumit siin
        public static void Configure(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=LAUREN\\SQLEXPRESS;Database=Cars;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True",
                b => b.MigrationsAssembly("Cars.Migrations")  // Määrame migratsioonide kogu projekti nime
            );
        }
    }
}
