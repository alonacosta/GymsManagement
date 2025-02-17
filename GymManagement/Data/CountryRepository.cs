﻿namespace GymManagement.Data
{
    using GymManagement.Data.Entities;
    using GymManagement.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        private readonly DataContext _context;

        public CountryRepository(DataContext context) : base(context) 
        {
            _context = context;
        }

        public IQueryable GetCountriesWithCities()
        {
            return _context.Countries
                .Include(c => c.Cities)
                .OrderBy(c => c.Name);
        }

        public IQueryable GetCountriesWithCitiesAndGyms()
        {
            return _context.Countries
                .Include(c => c.Cities)
                .ThenInclude(c => c.Gyms)               
                .OrderBy(c => c.Name);
        }

        //public IQueryable GetCountriesWithCitiesAndGymsForSearch(string searchCountry)
        //{
        //    return _context.Countries
        //   .Include(c => c.Cities)
        //   .ThenInclude(c => c.Gyms)
        //   .Where(c => string.IsNullOrWhiteSpace(searchCountry) ||
        //                c.Name.Contains(searchCountry, StringComparison.OrdinalIgnoreCase))
        //   .OrderBy(c => c.Name);
        //}

        public async Task<Country> GetCountryWithCitiesAsync(int id)
        {
            return await _context.Countries
                .Include(c => c.Cities)
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Country> GetCountryAsync(City city)
        {
            return await _context.Countries
                .Where(c => c.Cities
                .Any(ci => ci.Id == city.Id))
                .FirstOrDefaultAsync();
        }

        public IEnumerable<SelectListItem> GetComboCountries()
        {
            var list = _context.Countries.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString(),

            }).OrderBy(l => l.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a country ...)",
                Value = "0",
            });

            return list;
        }

        public async Task AddCityAsync(CityViewModel model)
        {
            var country = await this.GetCountryWithCitiesAsync(model.CountryId);

            if (country == null) 
            {
                return;
            }

            country.Cities.Add(new City
            {
                Name = model.Name,
            });

            _context.Countries.Update(country);
            await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteCityAsync(City city)
        {
            var country = await _context.Countries
                .Where(c => c.Cities
                .Any(ci => ci.Id == city.Id))
                .FirstOrDefaultAsync();

            if (country == null) 
            {
                return 0;
            }

            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();

            return country.Id;
        }

        public async Task<City> GetCityAsync(int id)
        {
            return await _context.Cities.FindAsync(id);
        }

        public async Task<int> UpdateCityAsync(City city)
        {
            var country = await _context.Countries
                .Where(c => c.Cities
                .Any(ci => ci.Id == city.Id))
                .FirstOrDefaultAsync();

            if (country == null) 
            {
                return 0;
            }

            _context.Cities.Update(city);
            await _context.SaveChangesAsync();

            return country.Id;
        }

        public IEnumerable<SelectListItem> GetComboCities(int countryId)
        {
            var country = _context.Countries.Find(countryId);

            var list = new List<SelectListItem>();

            if (country != null) 
            { 
                list = _context.Cities.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),

                }).OrderBy(l => l.Text).ToList();

                list.Insert(0, new SelectListItem
                {
                    Text = "(Select a city ...)",
                    Value = "0",
                });
            }

            return list;
        }
    }
}
