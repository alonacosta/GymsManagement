namespace GymManagement.Data
{
    using GymManagement.Data.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface IGymRepository : IGenericRepository<Gym>
    {
        IEnumerable<SelectListItem> GetComboGyms(int cityId);

        Task<IEnumerable<Gym>> GetGymsByCityId(int cityId);

        IQueryable GetGymsWithCities();

        IQueryable GetGymsWithCitiesFromCountry(int countryId);

        Task<Gym> GetGymWithCityAsync(int id);

        IEnumerable<SelectListItem> GetComboGymEquipments();

        Task<GymEquipment> GetGymEquipmentByIdAsync(int id);

        Task<GymEquipment> GetGymEquipmentByGymIdAsync(int gymId);

        Task AddGymEquipmentAsync(GymEquipment gymEquipment);

        Task EditGymEquipmentAsync(GymEquipment gymEquipment);

        Task DeleteGymEquipmentAsync(GymEquipment gymEquipment);
    }
}
