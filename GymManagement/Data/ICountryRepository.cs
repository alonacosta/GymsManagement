namespace GymManagement.Data
{
    using GymManagement.Data.Entities;
    using GymManagement.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface ICountryRepository : IGenericRepository<Country>
    {
        IQueryable GetCountriesWithCities();

        Task<Country> GetCountryWithCitiesAsync(int id);

        Task<Country> GetCountryAsync(City city);

        IEnumerable<SelectListItem> GetComboCountries();

        Task<City> GetCityAsync(int id);

        Task AddCityAsync(CityViewModel model);

        Task<int> UpdateCityAsync(City city);

        Task<int> DeleteCityAsync(City city);

        IEnumerable<SelectListItem> GetComboCities(int countryId);
    }
}
