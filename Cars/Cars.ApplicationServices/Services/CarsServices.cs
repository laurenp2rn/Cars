using Cars.Core.Domain;
using Cars.Core.ServiceInterface;
using Cars.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cars.ApplicationServices.Services
{
    public class CarsServices : ICarsServices
    {
        private readonly CarsContext _context;

        public CarsServices(CarsContext context)
        {
            _context = context;
        }

        public async Task<List<Car>> GetAllCarsAsync()
        {
            return await _context.Cars
                                 .Where(car => !car.Deleted)
                                 .ToListAsync();
        }

        public async Task<Car> GetCarByIdAsync(int id)
        {
            return await _context.Cars
                                 .FirstOrDefaultAsync(car => car.Id == id && !car.Deleted);
        }

        public async Task AddCarAsync(Car car)
        {
            car.CreatedAt = DateTime.UtcNow;
            car.ModifiedAt = DateTime.UtcNow;
            car.Deleted = false;
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCarAsync(Car car)
        {
            car.ModifiedAt = DateTime.UtcNow;
            _context.Cars.Update(car);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteCarAsync(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car != null)
            {
                car.Deleted = true;
                _context.Cars.Update(car);
                await _context.SaveChangesAsync();
            }
        }

        public async Task CreateCarAsync(Car car)
        {
            car.CreatedAt = DateTime.UtcNow;
            car.ModifiedAt = DateTime.UtcNow;
            car.Deleted = false;

            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
        }

        public Task CreateCarAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
