namespace GymManagement.Controllers
{
    using GymManagement.Data;
    using GymManagement.Data.Entities;
    using GymManagement.Helpers;
    using GymManagement.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Vereyon.Web;

    public class SessionsController : Controller
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IConverterHelper _converterHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IUserHelper _userHelper;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IFlashMessage _flashMessage;

        public SessionsController(ISessionRepository sessionRepository, IConverterHelper converterHelper, 
            IBlobHelper blobHelper, IUserHelper userHelper,
            IAppointmentRepository appointmentRepository,
            IFlashMessage flashMessage)
        {
            _sessionRepository = sessionRepository;
            _converterHelper = converterHelper;
            _blobHelper = blobHelper;
            _userHelper = userHelper;
            _appointmentRepository = appointmentRepository;
            _flashMessage = flashMessage;
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

        public async Task<IActionResult> BookSession(int? id)
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
        public async Task<IActionResult> BookSession(int? id, Session session)
        {
            if (id == null)
            {
                return NotFound();
            }

           session = await _sessionRepository.GetByIdAsync(id.Value);

            if (session == null)
            {
                return NotFound();
            }           

            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            if(user == null) { return NotFound(); } 

            var client = await _appointmentRepository.GetClientByUserIdAsync(user.Id);

            if (await _appointmentRepository.IsClientHasAppointmentAsync(client.Id, session.Name, session.Id))
            {
                _flashMessage.Danger("You already have an appointment booked in this Session!");
                return View(session);
            }

            session.Capacity--;

            await _sessionRepository.UpdateAsync(session);

            var appointmentTemp = new AppointmentTemp
            {
                Client = client,
                Name = session.Name,
                StartSession = session.StartSession,
                EndSession = session.EndSession,
                RemainingCapacity = session.Capacity,
            };

            await _appointmentRepository.AddAppointmentTempAsync(appointmentTemp);

            return RedirectToAction("BookAwait", "Appointments");
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

                    await _sessionRepository.UpdateSessionWithAppointmentsAsync(model.Id, session);
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
