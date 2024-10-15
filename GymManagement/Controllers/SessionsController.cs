namespace GymManagement.Controllers
{
    using GymManagement.Data;
    using GymManagement.Helpers;
    using GymManagement.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class SessionsController : Controller
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IConverterHelper _converterHelper;
        private readonly IBlobHelper _blobHelper;

        public SessionsController(ISessionRepository sessionRepository, IConverterHelper converterHelper, 
            IBlobHelper blobHelper)
        {
            _sessionRepository = sessionRepository;
            _converterHelper = converterHelper;
            _blobHelper = blobHelper;
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
        public async Task<IActionResult> Create(SessionViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0) 
                { 
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "sessions");
                }

                var session = _converterHelper.ToSession(model, imageId, true);

                await _sessionRepository.CreateAsync(session);
                
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

            var session = await _sessionRepository.GetByIdAsync(id.Value);

            var model = _converterHelper.ToSessionViewModel(session);

            if (session == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SessionViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = model.ImageId;

                try
                {
                    if (model.ImageFile != null && model.ImageFile.Length > 0) 
                    {
                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "sessions");
                    }

                    var session = _converterHelper.ToSession(model, imageId, false);

                    await _sessionRepository.UpdateAsync(session);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _sessionRepository.ExistAsync(model.Id))
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
