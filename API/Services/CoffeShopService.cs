using API.Models;
using DataAccess.Data;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class CoffeShopService : ICoffeShopService
    {
        private readonly ApplicationDbContext _context;

        public CoffeShopService(ApplicationDbContext context)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<List<CoffeShopModel>> GetCoffeShopsAsync()
        {
            var iems = await (from s in _context.CoffeShops 
                              select new CoffeShopModel()
                              { 
                                  Id = s.Id,
                                  Name = s.Name,
                                  Address = s.Address
                              }).ToListAsync();

            return iems;
        }
    }
}
