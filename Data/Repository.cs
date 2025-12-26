using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FilmRentalSystem.Models;

namespace FilmRentalSystem.Data
{
    public class Repository : IDisposable
    {
        private readonly AppDbContext _context;

        public Repository()
        {
            _context = new AppDbContext();
        }

        // Методы для Cinema
        public async Task<List<Cinema>> GetAllCinemasAsync() => await _context.Cinemas.ToListAsync();
        public async Task<Cinema> GetCinemaByIdAsync(int id) => await _context.Cinemas.FindAsync(id);

        public async Task AddCinemaAsync(Cinema cinema)
        {
            await _context.Cinemas.AddAsync(cinema);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCinemaAsync(Cinema cinema)
        {
            _context.Cinemas.Update(cinema);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCinemaAsync(int id)
        {
            var cinema = await _context.Cinemas.FindAsync(id);
            if (cinema != null)
            {
                _context.Cinemas.Remove(cinema);
                await _context.SaveChangesAsync();
            }
        }

        // Методы для Supplier
        public async Task<List<Supplier>> GetAllSuppliersAsync() =>
            await _context.Suppliers.Include(s => s.Movies).ToListAsync();

        public async Task<Supplier> GetSupplierByIdAsync(int id) =>
            await _context.Suppliers.Include(s => s.Movies).FirstOrDefaultAsync(s => s.Id == id);

        public async Task AddSupplierAsync(Supplier supplier)
        {
            await _context.Suppliers.AddAsync(supplier);
            await _context.SaveChangesAsync();
        }

        // Методы для Movie
        public async Task<List<Movie>> GetAllMoviesAsync() =>
            await _context.Movies.Include(m => m.Supplier).ToListAsync();

        public async Task<List<Movie>> GetMoviesByCategoryAsync(string category) =>
            await _context.Movies.Where(m => m.Category == category).Include(m => m.Supplier).ToListAsync();

        public async Task AddMovieAsync(Movie movie)
        {
            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMovieAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
            }
        }

        // Методы для Rental
        public async Task<List<Rental>> GetAllRentalsAsync() =>
            await _context.Rentals.Include(r => r.Movie).Include(r => r.Cinema).ToListAsync();

        public async Task<List<Rental>> GetActiveRentalsAsync() =>
            await _context.Rentals.Where(r => !r.IsReturned).Include(r => r.Movie).Include(r => r.Cinema).ToListAsync();

        public async Task<List<Rental>> GetOverdueRentalsAsync() =>
            await _context.Rentals.Where(r => r.EndDate < DateTime.Now && !r.IsReturned).Include(r => r.Movie).Include(r => r.Cinema).ToListAsync();

        public async Task AddRentalAsync(Rental rental)
        {
            await _context.Rentals.AddAsync(rental);
            await _context.SaveChangesAsync();
        }

        public async Task ReturnRentalAsync(int rentalId)
        {
            var rental = await _context.Rentals.FindAsync(rentalId);
            if (rental != null)
            {
                rental.IsReturned = true;
                if (rental.EndDate < DateTime.Now)
                {
                    var daysOverdue = (DateTime.Now - rental.EndDate).Days;
                    rental.PenaltyFee = rental.RentalFee * 0.1m * daysOverdue;
                }
                await _context.SaveChangesAsync();
            }
        }

        // Статистика
        public async Task<decimal> GetTotalRentalIncomeAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.Rentals.AsQueryable();
            if (startDate.HasValue) query = query.Where(r => r.StartDate >= startDate.Value);
            if (endDate.HasValue) query = query.Where(r => r.StartDate <= endDate.Value);
            return await query.SumAsync(r => r.RentalFee + r.PenaltyFee);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}