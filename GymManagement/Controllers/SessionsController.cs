namespace GymManagement.Controllers
{
    using GymManagement.Data;
    using GymManagement.Data.Entities;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class SessionsController : Controller
    {
        private readonly ISessionRepository _sessionRepository;

        public SessionsController(ISessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        public IActionResult Index()
        {
            return View(_sessionRepository.GetAll().OrderBy(s => s.Name));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var session = await _sessionRepository.GetByIdAsync(id.Value);
                
            if (session == null)
            {
                return NotFound();
            }

            return View(session);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Session session)
        {
            if (ModelState.IsValid)
            {
                await _sessionRepository.CreateAsync(session);
                
                return RedirectToAction(nameof(Index));
            }

            return View(session);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var session = await _sessionRepository.GetByIdAsync(id.Value);

            if (session == null)
            {
                return NotFound();
            }

            return View(session);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Session session)
        {
            if (id != session.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _sessionRepository.UpdateAsync(session);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _sessionRepository.ExistAsync(session.Id))
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
            return View(session);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var session = await _sessionRepository.GetByIdAsync(id.Value);
                
            if (session == null)
            {
                return NotFound();
            }

            return View(session);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var session = await _sessionRepository.GetByIdAsync(id);

            if (session != null)
            {
                await _sessionRepository.DeleteAsync(session);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
