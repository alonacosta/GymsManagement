using GymManagement.Data;
using GymManagement.Helpers;
using GymManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GymManagement.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Syncfusion.EJ2.Linq;
using System.Linq;

namespace GymManagement.Controllers
{
    public class GymsController : Controller
    {
        private readonly IGymRepository _gymRepository;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly ICountryRepository _countryRepository;
        private readonly IGymSessionRepository _gymSessionRepository;
        private readonly IUserHelper _userHelper;
        private readonly IEquipmentsRepository _equipmentsRepository;

        public GymsController(IGymRepository gymRepository,
            IBlobHelper blobHelper,
            IConverterHelper converterHelper,
            ICountryRepository countryRepository,
            IGymSessionRepository gymSessionRepository,
            IUserHelper userHelper,           
            IEquipmentsRepository equipmentsRepository)
        {
            _gymRepository = gymRepository;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
            _countryRepository = countryRepository;
            _gymSessionRepository = gymSessionRepository;
            _userHelper = userHelper;
            _equipmentsRepository = equipmentsRepository;   
        }

        public IActionResult Index()
        {
            return View(_gymRepository.GetGymsWithCities());
        }

        [Authorize]
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

        //[Authorize]
        public IActionResult Create()
        {
            var model = new GymViewModel
            {
                Countries = _countryRepository.GetComboCountries(),
                Cities = _countryRepository.GetComboCities(0),
            };
            return View(model);
        }

        [HttpPost]
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

            var gymEquipment = await _gymRepository.GetGymEquipmentByGymIdAsync(id.Value);

            var gymEquipmentDetails = _gymRepository.GetGymEquipmentsByGymId(id.Value);

            var gymEquipmentsList = new List<GymEquipmentViewModel>();

            foreach (var item in gymEquipmentDetails) 
            {
                var equipmentItem = new GymEquipmentViewModel
                {
                    Id = item.Id,
                    EquipmentName = item.EquipmentName,
                    GymId = item.GymId,
                    EquipmentId = item.EquipmentId,
                };

                gymEquipmentsList.Add(equipmentItem);
            }

            model.GymEquipmentDetails = gymEquipmentsList;

            //if (gymEquipment != null)
            //{
            //    var equipment = await _equipmentsRepository.GetEquipmentByGymEquipmentIdAsync(gymEquipment.Id);

            //    if (equipment != null)
            //    {
            //        model.GymEquipment = new GymEquipmentViewModel
            //        {
            //            Id = gymEquipment.Id,
            //            EquipmentName = gymEquipment.EquipmentName,
            //            GymId = gymEquipment.GymId,
            //            EquipmentId = gymEquipment.EquipmentId,
            //        };
            //    }
            //}
            //else
            //{
            //    model.GymEquipment = null;
            //}

            model.Countries = _countryRepository.GetComboCountries();
            model.Cities = model.Cities ?? _countryRepository.GetComboCities(model.CountryId ?? 0);

            return View(model);
        }

        [HttpPost]
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

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gym = await _gymRepository.GetGymWithCityAsync(id.Value);
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

            try
            {
                if (gym != null)
                {
                    await _gymRepository.DeleteAsync(gym);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
                {
                    ViewBag.ErrorTitle = $"{gym.Name} is probably being used!!!";
                    ViewBag.ErrorMessage = $"{gym.Name} can't be deleted because there are sessions that use it <br/>" +
                    $"First try deleting all the sessions that are using it," +
                    $" and delete it again";
                }
                return View("Error");
            }
        }

        // GET: Gyms/ChooseCountry
        public IActionResult ChooseCountry() 
        {
            var countries = _countryRepository.GetCountriesWithCitiesAndGyms();
            
            return View(countries); 
        }

        // GET: Gyms/GymFromCountry/countryId=5
        public IActionResult GymsFromCountry(int? countryId)
        {
            if (countryId == null) { return NotFound(); }

            var gyms = _gymRepository.GetGymsWithCitiesFromCountry(countryId.Value);

            ViewData["CountryId"] = countryId;
            return View(gyms);            
        }

        // GET: Gyms/GymGetails/countryId=5
        public async Task<IActionResult> GymDetails(int? countryId, int? gymId)
        {
            if (countryId == null) { return NotFound(); }
            if (gymId == null) { return NotFound(); }

            ViewData["CountryId"] = countryId;
            ViewData["GymId"] = gymId;

            var gym = await _gymRepository.GetGymWithCityAsync(gymId.Value);

            var employees = await _gymRepository.GetEmployeesFromGymAsync(gym.Id);

            var model = new GymDetailsViewModel
            {
                GymId = gymId.Value,
                Name = gym.Name,
                Address = gym.Address,
                City = gym.City,
                ImageGymId = gym.ImageId,
                ImageGymUrl = gym.ImageFullPath,
                Employees = employees,
            }; 

            return View(model);
        }

        /*public IActionResult GetSessionsFromGym(int? gymId, int? countryId)
        {
            if(gymId == null) { return NotFound(); }
            if (countryId == null) { return NotFound(); }

            ViewData["CountryId"] = countryId;
            ViewData["GymId"] = gymId;
            
            var sessions = _gymSessionRepository.GetGymSessions(gymId.Value);
            return View(sessions);
        }*/

        
        public IActionResult GetSessionsFromGym(int? gymId, int? countryId, bool isOnline, bool isGroup, DateTime? startTime)
        {
            if(gymId == null) { return NotFound(); }
            if (countryId == null) { return NotFound(); }

            ViewData["CountryId"] = countryId;
            ViewData["GymId"] = gymId;
            ViewData["isOnline"] = isOnline;
            ViewData["isGroup"] = isGroup;

            var sessions = _gymSessionRepository.GetGymSessions(gymId.Value, startTime);
            if (isOnline == true && isGroup == true) 
            {
                sessions = _gymSessionRepository.GetGroupnOnlineGymSessions(gymId.Value, startTime);
            } else if (isOnline == true) 
            {
                sessions = _gymSessionRepository.GetOnlineGymSessions(gymId.Value, startTime);
            }
            else if (isGroup == true)
            {
                sessions = _gymSessionRepository.GetGroupGymSessions(gymId.Value, startTime);
            }
            //
            return View(sessions);
        }

        public async Task<IActionResult> GetClassesMap(int? gymId, int? countryId)
        {
            if (gymId == null) { return NotFound(); }
            if (countryId == null) { return NotFound(); }   
            
            bool isClient = false;

            if (this.User.Identity.IsAuthenticated)
            {
                var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                
                if (user == null) { return NotFound(); }

                 isClient = await _gymRepository.IsUserIsClient(user.Id, gymId.Value);  
            }

            ViewData["CountryId"] = countryId;
            ViewData["GymId"] = gymId;
            ViewData["IsClient"] = isClient;
            ViewBag.IsClient = isClient;
           
            var appData = new List<AppointmentData>();            

            var sessions = _gymSessionRepository.GetGymSessionsById(gymId.Value);

           
            foreach ( var session in sessions)
            {
                appData.Add(new AppointmentData
                {
                    Id = session.Id,
                    Subject = session.Session.Name,
                    StartTime = session.StartSession,
                    EndTime = session.EndSession,
                    IsReadonly = session.StartSession >= DateTime.Now ? false : true,
                    ImageName = session.Session.Name,
                    Url = session.Session.ImageFullPath,
                    PrimaryColor = "#FFFFFF",
                    SecondaryColor = "#FF4858",
                    StateButton = session.StartSession < DateTime.Now ? "disabled" : "",
                });
            }
            return View(appData);
        }

        [HttpPost]
        [Route("Gyms/GetCitiesAsync")]
        public async Task<JsonResult> GetCitiesAsync(int countryId)
        {
            var country = await _countryRepository.GetCountryWithCitiesAsync(countryId);
            var cities = country.Cities
                .OrderBy(c => c.Name)
                .Select(c => new { id = c.Id, name = c.Name })
                .ToList();

            return Json(cities);
            //return Json(country.Cities.OrderBy(c => c.Name));
        }

        public IActionResult GymNotFound()
        {
            return View();
        }

        public async Task<IActionResult> AddGymEquipment(int id)
        {
            var gym = await _gymRepository.GetByIdAsync(id);

            if (gym == null)
            {
                return NotFound();
            }

            var model = new GymEquipmentViewModel
            {
                GymId = gym.Id,
                Equipments = _equipmentsRepository.GetComboEquipments(),
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddGymEquipment(GymEquipmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var gymEquipment = new GymEquipment
                {
                    GymId = model.GymId,
                    EquipmentId = model.EquipmentId,
                    IsActive = true,
                };

                await _gymRepository.AddGymEquipmentAsync(gymEquipment);

                return RedirectToAction("Index");
            }

            model.Equipments = _equipmentsRepository.GetComboEquipments();

            return View(model);
        }

        public async Task<IActionResult> EditGymEquipment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymEquipment = await _gymRepository.GetGymEquipmentByIdAsync(id.Value);

            if (gymEquipment == null)
            {
                return NotFound();
            }

            var model = new GymEquipmentViewModel
            {
                Id = gymEquipment.Id,
                GymId = gymEquipment.GymId,
                EquipmentId = gymEquipment.EquipmentId,
                Equipments = _equipmentsRepository.GetComboEquipments(),
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditGymEquipment(int id, GymEquipmentViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var gymEquipment = await _gymRepository.GetGymEquipmentByIdAsync(id);

                if (gymEquipment == null)
                {
                    return NotFound();
                }

                gymEquipment.GymId = model.GymId;
                gymEquipment.EquipmentId = model.EquipmentId;

                try
                {
                    await _gymRepository.EditGymEquipmentAsync(gymEquipment);
                }
                catch
                {
                    if (!await _gymRepository.ExistAsync(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("Index");
            }

            model.Equipments = _equipmentsRepository.GetComboEquipments();

            return View(model);
        }

        public async Task<IActionResult> DeleteGymEquipment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymEquipment = await _gymRepository.GetGymEquipmentByIdAsync(id.Value);

            if (gymEquipment == null)
            {
                return NotFound();
            }

            var equipment = await _equipmentsRepository.GetEquipmentByGymEquipmentIdAsync(gymEquipment.Id);

            if (equipment == null)
            {
                return NotFound();
            }

            return View(gymEquipment);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteGymEquipment(int id)
        {
            var gymEquipment = await _gymRepository.GetGymEquipmentByIdAsync(id);

            if (gymEquipment != null)
            {
                try
                {
                    await _gymRepository.DeleteGymEquipmentAsync(gymEquipment);

                    return RedirectToAction("Index");
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
                    {
                        ViewBag.ErrorTitle = $"{gymEquipment.Equipment.Name} is probably being used!!!";
                        ViewBag.ErrorMessage = $"{gymEquipment.Equipment.Name} can't be deleted because there are gyms that use it <br/>" +
                        $"First try deleting all the gyms that are using it," +
                        $" and delete it again";
                    }

                    return View("Error");
                }
            }

            return NotFound();
        }
    }
}