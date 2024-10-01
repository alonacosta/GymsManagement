using GymManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Data
{
    public class GymRepository : GenericRepository<Gym>, IGymRepository
    {
        private readonly DataContext _context;

        public GymRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable GetGymsWithCities()
        {
            return _context.Gyms
                .Include(g => g.City)                
                .OrderBy(g => g.Name);
        }

        public async Task<Gym> GetGymWithCityAsync(int id)
        {
            return await _context.Gyms
                .Include(g => g.City)
                .Where(g => g.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}
