using GymManagement.Data;
using GymManagement.Data.Entities;
using GymManagement.Helpers;
using GymManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vereyon.Web;

namespace GymManagement.Controllers
{
    public class GymSessionsController : Controller
    {
        private readonly IGymSessionRepository _gymSessionRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IConverterHelper _converterHelper;
        private readonly IGymRepository _gymRepository;
        private readonly IFlashMessage _flashMessage;
        private readonly IUserHelper _userHelper;
        private readonly IAppointmentRepository _appointmentRepository;

        public GymSessionsController(IGymSessionRepository gymSessionRepository,
            ISessionRepository sessionRepository,
            IConverterHelper converterHelper,
            IGymRepository gymRepository,
            IFlashMessage flashMessage,
            IUserHelper userHelper,
            IAppointmentRepository appointmentRepository)
        {
            _gymSessionRepository = gymSessionRepository;
            _sessionRepository = sessionRepository;
            _converterHelper = converterHelper;
            _gymRepository = gymRepository;
            _flashMessage = flashMessage;
            _userHelper = userHelper;
            _appointmentRepository = appointmentRepository;
        }

        // GET: GymSessions/5
        [Authorize(Roles = "Admin")]
        public async Task< IActionResult> Index(int? gymId)
        {
            if (gymId == null) { return NotFound(); }

            var gym = await _gymRepository.GetByIdAsync(gymId.Value);  
            if(gym == null) { return NotFound(); }

            ViewData["GymId"] = gymId;
            ViewData["GymName"] = $"{gym.Name} - {gym.Address}";            

            return View(_gymSessionRepository.GetGymSessions(gymId.Value, null));
        }

        // GET: GymSessions/Create/5
        [Authorize(Roles = "Admin")]
        public IActionResult Create(int gymId)
        {
            var model = new GymWithSessionViewModel
            {
                GymId = gymId,
                Sessions = _sessionRepository.GetComboSessions()
            };
            
            return View(model);
        }

        // POST: GymSessions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(GymWithSessionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var gymSession = _converterHelper.ToGymSession(model, true);

              await _gymSessionRepository.CreateAsync(gymSession);
                return RedirectToAction("Index", new {gymId = model.GymId});
            }
            return View(model);
        }

        // GET: GymSessions/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymSession = await _gymSessionRepository.GetByIdAsync(id.Value);

            if (gymSession == null) { return NotFound(); }

            var model = _converterHelper.ToGymWithSessionViewModel(gymSession);
            model.Sessions = _sessionRepository.GetComboSessions();

            return View(model);
        }

        // POST: Gyms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(GymWithSessionViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var gymSession = _converterHelper.ToGymSession(model, false);

                    //await _gymSessionRepository.UpdateAsync(gymSession);
                    await _gymSessionRepository.UpdateSessionWithAppointmentsAsync(model.Id, gymSession);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _gymSessionRepository.ExistAsync(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new {gymId = model.GymId});
            }
            return View(model);
        }

        //GET: GymSessions/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymSession = await _gymSessionRepository.GetGymSessionByIdAsync(id.Value);
            if (gymSession == null)
            {
                return NotFound();
            }

            return View(gymSession);
        }

        // POST: GymSessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gymSession = await _gymSessionRepository.GetByIdAsync(id);
            try
            {
                if (gymSession != null)
                {
                    await _gymSessionRepository.DeleteAsync(gymSession);
                }

                return RedirectToAction("Index", new { gymId = gymSession.GymId });
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
                {
                    ViewBag.ErrorTitle = $"This session is probably being used!!!";
                    ViewBag.ErrorMessage = $"This session can't be deleted because there are appointments that use it <br/>" +
                    $"First try deleting all the appointments that are using it," +
                    $" and delete it again";
                }
                return View("Error");
            }
        }

        [Authorize(Roles = "Client")]
        public async Task<IActionResult> BookSession(int? id, int? gymId, int? countryId)
        {
            if (id == null || gymId == null || countryId == null)
            {
                return NotFound();
            }

            ViewData["CountryId"] = countryId;
            ViewData["GymId"] = gymId;

            var gymSession = await _gymSessionRepository.GetGymSessionByIdAsync(id.Value);

            if (gymSession == null)
            {
                return NotFound();
            }

            return View(gymSession);
        }

        [HttpPost]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> BookSession(int? id, int? gymId, int? countryId, GymSession gymSession)
        {
            if (id == null || gymId == null || countryId == null)
            {
                return NotFound();
            }

            ViewData["CountryId"] = countryId;
            ViewData["GymId"] = gymId;

            gymSession = await _gymSessionRepository.GetGymSessionByIdAsync(id.Value);

            if (gymSession == null)
            {
                return NotFound();
            }

            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            if (user == null) { return NotFound(); }

            var client = await _appointmentRepository.GetClientByUserIdAsync(user.Id);

            if (await _appointmentRepository.IsClientHasAppointmentAsync(client.Id, gymSession.StartSession, gymSession.Session.Name, gymSession.SessionId))
            {
                _flashMessage.Danger("You already have an appointment booked in this Session!");                
                
                return View(gymSession);
            }

            if (gymSession.Capacity <= 0)
            {
                _flashMessage.Danger("This session is fully booked.");
                return View(gymSession);
            }

            gymSession.Capacity--;

            await _gymSessionRepository.UpdateAsync(gymSession);

            var appointmentTemp = new AppointmentTemp
            {
                Client = client,
                Name = gymSession.Session.Name,
                StartSession = gymSession.StartSession,
                EndSession = gymSession.EndSession,
                RemainingCapacity = gymSession.Capacity,
            };

            await _appointmentRepository.AddAppointmentTempAsync(appointmentTemp);

            return RedirectToAction("BookAwait", "Appointments", new { countryId = countryId, gymId = gymId });
        }

        //public async Task<IActionResult> TryFreeSession(int? id, int? gymId, int? countryId)
        //{
        //    if (id == null || gymId == null || countryId == null)
        //    {
        //        return NotFound();
        //    }

        //    ViewData["CountryId"] = countryId;
        //    ViewData["GymId"] = gymId;

        //    var gymSession = await _gymSessionRepository.GetGymSessionByIdAsync(id.Value);

        //    if (gymSession == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(gymSession);
        //}
    }
}
