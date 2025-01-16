using System.Collections.Generic;
using System.Reflection.Emit;
using Cars.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Cars.Data
{
    public class CarsContext : DbContext
    {
        public CarsContext(DbContextOptions<CarsContext> options)
            : base(options)
        {
        }
        public DbSet<Car> Cars { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Car>().ToTable("Cars");
            modelBuilder.Entity<Car>()
                        .Property(c => c.CreatedAt)
                        .IsRequired();
            modelBuilder.Entity<Car>()
                        .Property(c => c.Model)
                        .HasColumnName("CarModel");
        }
    }
}
