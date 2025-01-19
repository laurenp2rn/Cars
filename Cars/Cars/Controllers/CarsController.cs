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

        public async Task<IActionResult> Index()
        {
            var cars = await _carsServices.GetAllCarsAsync();
            return View(cars); 
        }


    }
}
