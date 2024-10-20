namespace GymManagement.Controllers
{
    using GymManagement.Data;
    using GymManagement.Data.Entities;
    using GymManagement.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class CountriesController : Controller
    {
        private readonly ICountryRepository _countryRepository;

        public CountriesController(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public IActionResult Index()
        {
            return View(_countryRepository.GetCountriesWithCities());
        }

        public IActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Country country) 
        {
            if (ModelState.IsValid) 
            {
                try
                {
                    await _countryRepository.CreateAsync(country);
                    return RedirectToAction("Index");
                }
                catch (Exception) 
                { 
                   // TODO: insert flashMessage saying that the country already exists.
                }

                return View(country);
            }

            return View(country); 
        }

        public async Task<IActionResult> Details(int? id) 
        {
            if (id == null) 
            { 
                return NotFound();
            }

            var country = await _countryRepository.GetCountryWithCitiesAsync(id.Value);

            if (country == null) 
            { 
                return NotFound();
            }

            return View(country);
        }

        public async Task<IActionResult> Edit(int? id) 
        { 
            if (id == null) 
            { 
                return NotFound();
            }

            var country = await _countryRepository.GetByIdAsync(id.Value);

            if (country == null) 
            {
                return NotFound();
            }

            return View(country);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Country country) 
        { 
            if (ModelState.IsValid) 
            { 
                await _countryRepository.UpdateAsync(country);
                return RedirectToAction("Index");
            }

            return View(country);
        }

        public async Task<IActionResult> Delete(int? id) 
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _countryRepository.GetByIdAsync(id.Value);

            if (country == null)
            {
                return NotFound();
            }

            try
            {   
                await _countryRepository.DeleteAsync(country);
                return RedirectToAction("Index");
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
                {
                    ViewBag.ErrorTitle = $"{country.Name} is probably being used!!!";
                    ViewBag.ErrorMessage = $"{country.Name} can't be deleted because there are cities and gyms that use it <br/>" +
                    $"First try deleting all the gyms and cities that are using it," +
                    $" and delete it again";
                }

                return View("Error");
            }

        }

        public async Task<IActionResult> AddCity(int? id) 
        {
            if (id == null) 
            {
                return NotFound();
            }

            var country = await _countryRepository.GetByIdAsync(id.Value);

            if (country == null) 
            {
                return NotFound();
            }

            var model = new CityViewModel 
            { 
                CountryId = country.Id, 
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddCity(CityViewModel model) 
        {
            if (ModelState.IsValid) 
            { 
                await _countryRepository.AddCityAsync(model);

                return RedirectToAction("Index");
            }

            return View(model);
        }

        public async Task<IActionResult> EditCity(int? id) 
        { 
            if (id == null) 
            { 
                return NotFound(); 
            }

            var city = await _countryRepository.GetCityAsync(id.Value);

            if (city == null) 
            { 
                return NotFound(); 
            }

            return View(city);
        }

        [HttpPost]
        public async Task<IActionResult> EditCity(City city) 
        {
            if (ModelState.IsValid) 
            { 
                var countryId = await _countryRepository.UpdateCityAsync(city);

                if (countryId != 0) 
                { 
                    return RedirectToAction("Details", new { id = countryId });
                }
            }

            return View(city);
        }

        public async Task<IActionResult> DeleteCity (int? id) 
        {
            if (id == null) 
            {
                return NotFound();
            }

            var city = await _countryRepository.GetCityAsync(id.Value);

            if (city == null) 
            {
                return NotFound();
            }

            try
            {
                var countryId = await _countryRepository.DeleteCityAsync(city);

                return RedirectToAction("Details", new { id = countryId });
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
                {
                    ViewBag.ErrorTitle = $"{city.Name} is probably being used!!!";
                    ViewBag.ErrorMessage = $"{city.Name} can't be deleted because there are gyms that use it <br/>" +
                    $"First try deleting all the gyms that are using it," +
                    $" and delete it again";
                }

                return View("Error");
            }
        }
    }
}
