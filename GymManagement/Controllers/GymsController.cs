using GymManagement.Data;
using GymManagement.Helpers;
using GymManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Controllers
{
    public class GymsController : Controller
    {
        private readonly IGymRepository _gymRepository;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly ICountryRepository _countryRepository;

        public GymsController(IGymRepository gymRepository,
            IBlobHelper blobHelper,
            IConverterHelper converterHelper,
            ICountryRepository countryRepository)
        {
            _gymRepository = gymRepository;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
            _countryRepository = countryRepository;
        }

        // GET: Gyms
        public IActionResult Index()
        {
            return View(_gymRepository.GetGymsWithCities());
        }

        // GET: Gyms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var gym = await _gymRepository.GetByIdAsync(id.Value);
            var gym = await _gymRepository.GetGymWithCityAsync(id.Value);

            if (gym == null)
            {
                return NotFound();
            }

            return View(gym);
        }

        // GET: Gyms/Create
        public IActionResult Create()
        {
            var model = new GymViewModel
            {
                Countries = _countryRepository.GetComboCountries(),
                Cities = _countryRepository.GetComboCities(0),
            };
            return View(model);
        }

        // POST: Gyms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GymViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "gyms");
                }

                var gym = _converterHelper.ToGym(model, imageId, true);

                //TODO: adicionar city

                await _gymRepository.CreateAsync(gym);

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Gyms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gym = await _gymRepository.GetByIdAsync(id.Value);
            if (gym == null)
            {
                return NotFound();
            }

            var model = new GymViewModel
            {
                Id = gym.Id,
                Name = gym.Name,
                Address = gym.Address,
                CityId = gym.CityId,
                ImageId = gym.ImageId,
            };

            var city = await _countryRepository.GetCityAsync(gym.CityId);
            if (city != null)
            {
                var country = await _countryRepository.GetCountryAsync(city);

                if (country != null)
                {
                    model.CountryId = country.Id;
                    model.Cities = _countryRepository.GetComboCities(country.Id);
                }
            }
            model.Countries = _countryRepository.GetComboCountries();
            model.Cities = model.Cities ?? _countryRepository.GetComboCities(model.CountryId ?? 0);

            return View(model);
        }

        // POST: Gyms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(GymViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Guid imageId = model.ImageId;
                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "gyms");
                    }

                    var gym = _converterHelper.ToGym(model, imageId, false);

                    await _gymRepository.UpdateAsync(gym);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _gymRepository.ExistAsync(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Gyms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gym = await _gymRepository.GetByIdAsync(id.Value);
            if (gym == null)
            {
                return NotFound();
            }

            return View(gym);
        }

        // POST: Gyms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gym = await _gymRepository.GetByIdAsync(id);
            if (gym != null)
            {
                await _gymRepository.DeleteAsync(gym);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Route("Gyms/GetCitiesAsync")]
        public async Task<JsonResult> GetCitiesAsync(int countryId)
        {
            var country = await _countryRepository.GetCountryWithCitiesAsync(countryId);
            return Json(country.Cities.OrderBy(c => c.Name));
        }
    }
}