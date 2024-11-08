namespace GymManagement.Data
{
    using GymManagement.Data.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface IGymRepository : IGenericRepository<Gym>
    {
        IEnumerable<SelectListItem> GetComboGyms(int cityId);
        //IQueryable GetEmployeesFromGym(int gymId);
        Task<List<Employee>> GetEmployeesFromGymAsync(int gymId);
        //Task<IQueryable<Employee>> GetEmployeesFromGym(int gymId);
        Task<IEnumerable<Gym>> GetGymsByCityId(int cityId);

        IQueryable GetGymsWithCities();
        IQueryable GetGymsWithCitiesFromCountry(int countryId);      

        Task<Gym> GetGymWithCityAsync(int id);
        Task<bool> IsUserIsClient(string userId, int gymId);

        IEnumerable<SelectListItem> GetComboGymEquipments();

        IQueryable<GymEquipment> GetGymEquipmentsByGymId(int gymId);

        Task<GymEquipment> GetGymEquipmentByIdAsync(int id);

        Task<GymEquipment> GetGymEquipmentByGymIdAsync(int gymId);

        Task AddGymEquipmentAsync(GymEquipment gymEquipment);

        Task EditGymEquipmentAsync(GymEquipment gymEquipment);

        Task DeleteGymEquipmentAsync(GymEquipment gymEquipment);
    }
}
