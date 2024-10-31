using GymManagement.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagement.Data
{
    public interface IGymRepository : IGenericRepository<Gym>
    {
        IEnumerable<SelectListItem> GetComboGyms(int cityId);

        Task<IEnumerable<Gym>> GetGymsByCityId(int cityId);
        IQueryable GetGymsWithCities();
        IQueryable GetGymsWithCitiesFromCountry(int countryId);      
        Task<Gym> GetGymWithCityAsync(int id);
        Task<bool> IsUserIsClient(string userId, int gymId);
    }
}
