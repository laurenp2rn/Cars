using Cars.Core.Domain;

namespace Cars.Core.ServiceInterface
{
    public interface ICarsServices
    {
        Task<List<Car>> GetAllCarsAsync();
        Task<Car> GetCarByIdAsync(int id);
        Task AddCarAsync(Car car);
        Task UpdateCarAsync(Car car);
        Task SoftDeleteCarAsync(int id);
        Task CreateCarAsync(int id);
        Task CreateCarAsync(Car car);
    }
}
