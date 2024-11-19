using GymManagement.Data.Entities;
using Microsoft.AspNetCore.JsonPatch.Internal;
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

        public async Task<List<Employee>> GetEmployeesFromGymAsync(int gymId)
        {
            return await _context.Employees
                .Include(e => e.User)
                .Include(e => e.Gym)
                .Include(e => e.Position)
                .Where(e => e.Gym.Id == gymId)
                .OrderBy(e => e.User.FirstName)
                .ToListAsync();               
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

        public async Task<bool> IsUserIsClient(string userId, int gymId)
        {
           var isClient = await _context.Clients
                .Include(c => c.User)
                .Include(c => c.Gym)
                .Where(c => c.User.Id == userId && c.Gym.Id == gymId)
                .FirstOrDefaultAsync();

            if(isClient == null)
            {
                return false;
            }
            return true;
        }

        public IEnumerable<SelectListItem> GetComboGymEquipments() 
        {
            var list = _context.GymEquipments
                .Select(ge => new SelectListItem
                {
                    Text = ge.Equipment.Name,
                    Value = ge.Id.ToString(),

                }).OrderBy(l => l.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select gym equipment ...)",
                Value = "0",
            });

            return list;
        }

        public IQueryable<GymEquipment> GetGymEquipmentsByGymId(int gymId) 
        {
            return _context.GymEquipments
                .Include(ge => ge.Gym)
                .Include(ge => ge.Equipment)
                .Where(ge => ge.Gym.Id == gymId)
                .OrderBy(ge => ge.Equipment.Name);
        }

        public async Task<GymEquipment> GetGymEquipmentByIdAsync(int id) 
        {
            return await _context.GymEquipments
                .FirstOrDefaultAsync(ge => ge.Id == id);
        }        

        public async Task<GymEquipment> GetGymEquipmentByGymIdAsync(int gymId) 
        {
            return await _context.GymEquipments
                .Include(ge => ge.Gym)
                .FirstOrDefaultAsync(ge => ge.GymId == gymId);
        }

        public async Task AddGymEquipmentAsync(GymEquipment gymEquipment) 
        { 
            _context.GymEquipments.Add(gymEquipment);

            await _context.SaveChangesAsync();
        }

        public async Task EditGymEquipmentAsync(GymEquipment gymEquipment) 
        { 
            var existingGymEquipment = await _context.GymEquipments.FindAsync(gymEquipment.Id);

            if (existingGymEquipment == null) 
            {
                throw new Exception("Gym equipment not found!");
            }

            existingGymEquipment.GymId = gymEquipment.GymId;
            existingGymEquipment.EquipmentId = gymEquipment.EquipmentId;

            _context.GymEquipments.Update(existingGymEquipment);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteGymEquipmentAsync(GymEquipment gymEquipment) 
        { 
            _context.GymEquipments.Remove(gymEquipment);

            await _context.SaveChangesAsync();
        }
        
    }
}
