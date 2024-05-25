using API.Models;
namespace API.Services
{
    public interface ICoffeShopService
    {
        Task<List<CoffeShopModel>> GetCoffeShopsAsync();
    }
}
