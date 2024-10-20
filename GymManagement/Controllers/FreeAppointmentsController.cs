using GymManagement.Data;
using GymManagement.Data.Entities;
using GymManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vereyon.Web;

namespace GymManagement.Controllers
{
    public class FreeAppointmentsController : Controller
    {
        private readonly IFreeAppointmentRepository _freeAppointmentRepository;
        private readonly IFlashMessage _flashMessage;

        public FreeAppointmentsController(IFreeAppointmentRepository freeAppointmentRepository,
            IFlashMessage flashMessage)
        {
            _freeAppointmentRepository = freeAppointmentRepository;
            _flashMessage = flashMessage;
        }

        //GET: FreeAppointments/ManageFreeAppointments
        [Authorize(Roles = "Admin")]
        public IActionResult ManageFreeAppointments()
        {
            return View(_freeAppointmentRepository.GetAllNotCompleteFreeAppointments());
        }

        //GET: FreeAppointments/CompletedAppointments
        [Authorize(Roles = "Admin")]
        public IActionResult CompletedAppointments()
        {
            return View(_freeAppointmentRepository.GetCompleteFreeAppointments()); 
        }

        //GET: FreeAppointments/TryFreeSession
        public async Task<IActionResult> TryFreeSession()
        {                
            return View();
        }

        //POST: FreeAppointments/TryFreeSession
        [HttpPost]
        public async Task<IActionResult> TryFreeSession(FreeAppointment freeAppointment)
        {
            if (ModelState.IsValid)
            {  
                if(await _freeAppointmentRepository.HasFreeApointment(freeAppointment.Email))
                {
                    _flashMessage.Danger("You already have an appointment, our team vai contactar you em breve!");
                    return View(freeAppointment);
                }

                freeAppointment.IsComplete = false;

                await _freeAppointmentRepository.CreateAsync(freeAppointment);

                return RedirectToAction(nameof(ThankYou));
            }
            return View(freeAppointment);
        }

        //GET: FreeAppointments/ThankYou
        public async Task<IActionResult> ThankYou()
        {
            return View();
        }

        //GET: FreeAppointments/Complete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Complete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var freeAppointment = await _freeAppointmentRepository.GetByIdAsync(id.Value);            

            if (freeAppointment == null)
            {
                return NotFound();
            }

            return View(freeAppointment);
        }

        //POST: FreeAppointments/Complete/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Complete(int? id, FreeAppointment freeAppointment)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {               
                   //var appointment = await _freeAppointmentRepository.GetByIdAsync(id.Value);
                   // if (appointment == null) 
                   // {
                   //     return NotFound();
                   // }
                   
                    //appointment.IsComplete = freeAppointment.IsComplete;                   
                    await _freeAppointmentRepository.UpdateAsync(freeAppointment);
                
                return RedirectToAction(nameof(ManageFreeAppointments));
            }

            return View(freeAppointment);
        }

    }
}
