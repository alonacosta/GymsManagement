using GymManagement.Data;
using GymManagement.Helpers;
using GymManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Controllers
{
    public class GymSessionsController : Controller
    {
        private readonly IGymSessionRepository _gymSessionRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IConverterHelper _converterHelper;
        private readonly IGymRepository _gymRepository;

        public GymSessionsController(IGymSessionRepository gymSessionRepository,
            ISessionRepository sessionRepository,
            IConverterHelper converterHelper,
            IGymRepository gymRepository)
        {
            _gymSessionRepository = gymSessionRepository;
            _sessionRepository = sessionRepository;
            _converterHelper = converterHelper;
            _gymRepository = gymRepository;
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

            return View(_gymSessionRepository.GetGymSessions(gymId.Value));
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
        public async Task<IActionResult> Edit(GymWithSessionViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var gymSession = _converterHelper.ToGymSession(model, false);

                   await _gymSessionRepository.UpdateAsync(gymSession);
                   
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gymSession = await _gymSessionRepository.GetByIdAsync(id);
            if (gymSession != null)
            {
                await _gymSessionRepository.DeleteAsync(gymSession);
            }

            return RedirectToAction("Index", new {gymId = gymSession.GymId});
        }
    }
}
