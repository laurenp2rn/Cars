using Microsoft.AspNetCore.Mvc;
using Cars.Core.Domain;
using Cars.Core.ServiceInterface;
using System;
using System.Threading.Tasks;

namespace Cars.Controllers
{
    public class CarsController : Controller
    {
        private readonly ICarsServices _carsServices;

        public CarsController(ICarsServices carsServices)
        {
            _carsServices = carsServices;
        }

        public async Task<IActionResult> Index()
        {
            var cars = await _carsServices.GetAllCarsAsync();
            return View(cars);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Car car)
        {
            if (!ModelState.IsValid)
            {
                return View(car);
            }

            car.CreatedAt = DateTime.UtcNow;
            car.ModifiedAt = DateTime.UtcNow;

            await _carsServices.AddCarAsync(car);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var car = await _carsServices.GetCarByIdAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Car car)
        {
            if (id != car.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(car);
            }

            var existingCar = await _carsServices.GetCarByIdAsync(id);
            if (existingCar == null)
            {
                return NotFound();
            }

            existingCar.Make = car.Make;
            existingCar.Model = car.Model;
            existingCar.VehicleType = car.VehicleType;
            existingCar.Fuel = car.Fuel;
            existingCar.ModifiedAt = DateTime.UtcNow;

            await _carsServices.UpdateCarAsync(existingCar);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var car = await _carsServices.GetCarByIdAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _carsServices.SoftDeleteCarAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
