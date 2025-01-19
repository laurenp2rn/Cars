using Microsoft.AspNetCore.Mvc;
using Cars.ApplicationServices.Services;
using Cars.Core.Domain;
using Cars.Core.ServiceInterface;

namespace Cars.Controllers
{
    public class CarsController : Controller
    {
        private readonly ICarsServices _carsServices;

        public CarsController(ICarsServices carsServices)
        {
            _carsServices = carsServices;
        }

        // GET: Cars
        public async Task<IActionResult> Index()
        {
            var cars = await _carsServices.GetAllCarsAsync();
            return View(cars);
        }

        // GET: Cars/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(); // Kuvab vormi auto loomiseks
        }

        // POST: Cars/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Car car)
        {
            if (!ModelState.IsValid)
            {
                return View(car); // Kui valideerimine ebaõnnestub, kuvab sama vormi
            }

            car.CreatedAt = DateTime.UtcNow;
            car.ModifiedAt = DateTime.UtcNow;

            await _carsServices.AddCarAsync(car); // Kutsutakse teenuse loomise meetod
            return RedirectToAction(nameof(Index)); // Suunatakse tagasi autode nimekirja
        }
    }
}
