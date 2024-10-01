using GymManagement.Data.Entities;

namespace GymManagement.Data
{
    public interface IGymRepository : IGenericRepository<Gym>
    {
        IQueryable GetGymsWithCities();
        Task<Gym> GetGymWithCityAsync(int id);
    }
}
