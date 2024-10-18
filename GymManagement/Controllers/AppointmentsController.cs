using GymManagement.Data;
using GymManagement.Data.Entities;
using GymManagement.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IUserHelper _userHelper;

        public AppointmentsController(IAppointmentRepository appointmentRepository,
            IUserHelper userHelper)
        {
            _appointmentRepository = appointmentRepository;
            _userHelper = userHelper;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            var model = await _appointmentRepository.GetAppointmentsAsync(this.User.Identity.Name);
            return View(model);
        }

        // GET: Appointments/BookAwait
        public IActionResult BookAwait() 
        {
            return View();  
        }

        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> AppointmentsManagement()
        {
            var appointmentsTemp = _appointmentRepository.GetAppointmentsTemp();
            return View(appointmentsTemp);
        }

        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> ConfirmBookingAll()
        {
            var responde = await _appointmentRepository.ConfirmBookingAllAsync();
            if(responde)
            {
                return RedirectToAction("AppointmentsManagement", "Appointments"); 
            }
            return RedirectToAction("AppointmentsManagement", "Appointments");
        }

        [Authorize(Roles = "Client")]
        public async Task<IActionResult> CancelBooking(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var response = await _appointmentRepository.CancelBookingAsync(id.Value);
            if (response) 
            {              
                return RedirectToAction("Index", "Appointments");
            }
            return RedirectToAction("Index", "Appointments");

        }

        [Authorize(Roles = "Client")]
        public async Task<IActionResult> ConfirmBooking(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentTemp = await _appointmentRepository.GetAppointmentTempByIdAsync(id.Value);
            if (appointmentTemp == null)
            {
                return NotFound();
            }

            var response = await _appointmentRepository.ConfirmBookingAsync(appointmentTemp.Client.Id, appointmentTemp.Name);
            return RedirectToAction("AppointmentsManagement", "Appointments");
        }

        
    }
}
