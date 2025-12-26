using Microsoft.EntityFrameworkCore;
using FilmRentalSystem.Models;

namespace FilmRentalSystem.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Rental> Rentals { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=filmrental.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Supplier>()
                .HasMany(s => s.Movies)
                .WithOne(m => m.Supplier)
                .HasForeignKey(m => m.SupplierId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Rentals)
                .WithOne(r => r.Movie)
                .HasForeignKey(r => r.MovieId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cinema>()
                .HasMany(c => c.Rentals)
                .WithOne(r => r.Cinema)
                .HasForeignKey(r => r.CinemaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Movie>().HasIndex(m => m.Title);
            modelBuilder.Entity<Cinema>().HasIndex(c => c.Name);
            modelBuilder.Entity<Supplier>().HasIndex(s => s.Name);
            modelBuilder.Entity<Rental>().HasIndex(r => r.StartDate);
            modelBuilder.Entity<Rental>().HasIndex(r => r.EndDate);
        }
    }
}
