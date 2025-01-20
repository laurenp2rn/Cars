using Cars.Core.Domain;
using Cars.Core.ServiceInterface;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Cars.Controllers;

public class CarsControllerTests
{
    private readonly Mock<ICarsServices> _mockService;
    private readonly CarsController _controller;

    public CarsControllerTests()
    {
        _mockService = new Mock<ICarsServices>();
        _controller = new CarsController(_mockService.Object);
    }

    [Fact]
    public async Task Index_ReturnsView()
    {
        var cars = new List<Car> { new Car { Model = "Astra" }, new Car { Model = "BMW e36" } };
        _mockService.Setup(service => service.GetAllCarsAsync()).ReturnsAsync(cars);

        var result = await _controller.Index();

        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Create_RedirectsToIndex()
    {
        var car = new Car { Model = "Astra" };
        _mockService.Setup(service => service.AddCarAsync(It.IsAny<Car>())).Returns(Task.CompletedTask);

        var result = await _controller.Create(car);

        Assert.IsType<RedirectToActionResult>(result);
    }

    [Fact]
    public async Task Delete_RedirectsToIndex()
    {
        _mockService.Setup(service => service.SoftDeleteCarAsync(It.IsAny<int>())).Returns(Task.CompletedTask);

        var result = await _controller.DeleteConfirmed(1);

        Assert.IsType<RedirectToActionResult>(result);
    }

    [Fact]
    public async Task Edit_Get_ShouldReturnViewWithCar()
    {
        var car = new Car { Id = 1, Model = "Astra", Make = "Opel", Fuel = "Petrol" };
        _mockService.Setup(service => service.GetCarByIdAsync(1)).ReturnsAsync(car);

        var result = await _controller.Edit(1);

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<Car>(viewResult.Model);
        Assert.Equal(1, model.Id);
    }
}
