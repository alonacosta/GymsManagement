using GymManagement.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
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


        public IEnumerable<SelectListItem> GetComboGyms(int cityId)
        {
            var city = _context.Cities.Find(cityId);
            var list = new List<SelectListItem>();
            if (city != null)
            {
                list = _context.Gyms.Select(g => new SelectListItem
                {
                    Text = g.Name,
                    Value = g.Id.ToString()
                }).OrderBy(l => l.Text).ToList();

                list.Insert(0, new SelectListItem
                {
                    Text = "(Select a gym...)",
                    Value = "0"
                });
            }
            return list;
        }

        public async Task<IEnumerable<Gym>> GetGymsByCityId(int cityId)
        {
            var gyms = await _context.Gyms
                .Where(g => g.CityId == cityId)
                .ToListAsync();

            return gyms;           
        }

        public IQueryable GetGymsWithCities()
        {
            return _context.Gyms
                .Include(g => g.City)                
                .OrderBy(g => g.Name);
        }

        public IQueryable GetGymsWithCitiesFromCountry(int countryId)
        {
            return _context.Gyms
                .Include(g => g.City)
                .Where(c => c.City.Country.Id == countryId)
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
