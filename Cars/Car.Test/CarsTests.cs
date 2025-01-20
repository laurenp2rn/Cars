using Cars.ApplicationServices.Services;
using Cars.Core.Domain;
using Cars.Core.ServiceInterface;
using Cars.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Cars.Tests
{
    public class CarsServiceTests : IDisposable
    {
        private readonly CarsContext _context;
        private readonly ICarsServices _carsServices;

        public CarsServiceTests()
        {
            var options = new DbContextOptionsBuilder<CarsContext>()
                .UseInMemoryDatabase(databaseName: "CarsTestDatabase")
                .Options;

            _context = new CarsContext(options);
            _carsServices = new CarsServices(_context);

            // Clean up before each test
            _context.Cars.RemoveRange(_context.Cars);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            // Clean up after each test
            _context.Cars.RemoveRange(_context.Cars);
            _context.SaveChanges();
            _context.Dispose();
        }

        [Fact]
        public async Task CreateCarAsync_ShouldAddCar()
        {
            var car = new Car
            {
                Make = "Toyota",
                Model = "Corolla",
                VehicleType = "Sedan",
                Fuel = "Petrol",
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            };

            await _carsServices.AddCarAsync(car); // Don't assign the result to a variable

            var carInDb = await _context.Cars.FindAsync(car.Id);

            Assert.NotNull(carInDb);
            Assert.Equal("Toyota", carInDb.Make);
            Assert.Equal("Corolla", carInDb.Model);
        }

        [Fact]
        public async Task GetAllCarsAsync_ShouldReturnAllCars()
        {
            _context.Cars.AddRange(
                new Car { Make = "Ford", Model = "Focus", VehicleType = "Hatchback", Fuel = "Diesel", CreatedAt = DateTime.UtcNow, ModifiedAt = DateTime.UtcNow },
                new Car { Make = "Honda", Model = "Civic", VehicleType = "Sedan", Fuel = "Petrol", CreatedAt = DateTime.UtcNow, ModifiedAt = DateTime.UtcNow }
            );
            await _context.SaveChangesAsync();

            var cars = await _carsServices.GetAllCarsAsync();

            Assert.Equal(2, cars.Count());
        }

        [Fact]
        public async Task UpdateCarAsync_ShouldModifyCar()
        {
            var car = new Car { Make = "Nissan", Model = "Altima", VehicleType = "Sedan", Fuel = "Gas", CreatedAt = DateTime.UtcNow, ModifiedAt = DateTime.UtcNow };
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();

            car.Model = "Maxima";
            car.Fuel = "Electric";

            await _carsServices.UpdateCarAsync(car); // Don't assign the result to a variable

            var updatedCar = await _context.Cars.FindAsync(car.Id);
            Assert.Equal("Maxima", updatedCar.Model);
            Assert.Equal("Electric", updatedCar.Fuel);
        }

        [Fact]
        public async Task DeleteCarAsync_ShouldRemoveCar()
        {
            var car = new Car { Make = "BMW", Model = "X5", VehicleType = "SUV", Fuel = "Hybrid", CreatedAt = DateTime.UtcNow, ModifiedAt = DateTime.UtcNow };
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();

            await _carsServices.SoftDeleteCarAsync(car.Id); // Don't assign the result to a variable
            var deletedCar = await _context.Cars.FindAsync(car.Id);

            Assert.Null(deletedCar);
        }
    }
}
