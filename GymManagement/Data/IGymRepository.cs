using GymManagement.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagement.Data
{
    public interface IGymRepository : IGenericRepository<Gym>
    {
        IEnumerable<SelectListItem> GetComboGyms(int cityId);

        Task<IEnumerable<Gym>> GetGymsByCityId(int cityId);
        IQueryable GetGymsWithCities();
        Task<Gym> GetGymWithCityAsync(int id);
    }
}
