namespace GymManagement.Controllers
{
    using GymManagement.Data;
    using GymManagement.Data.Entities;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class EquipmentsController : Controller
    {
        private readonly IEquipmentsRepository _equipmentsRepository;

        public EquipmentsController(IEquipmentsRepository equipmentsRepository)
        {
            _equipmentsRepository = equipmentsRepository;
        }

        public IActionResult Index()
        {
            return View(_equipmentsRepository.GetAll().OrderBy(e => e.Name));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _equipmentsRepository.GetByIdAsync(id.Value);
            
            if (equipment == null)
            {
                return NotFound();
            }

            return View(equipment);
        }
    
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Equipment equipment)
        {
            if (ModelState.IsValid)
            {
                await _equipmentsRepository.CreateAsync(equipment);

                return RedirectToAction("Index");
            }

            return View(equipment);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _equipmentsRepository.GetByIdAsync(id.Value);

            if (equipment == null)
            {
                return NotFound();
            }

            return View(equipment);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Equipment equipment)
        {
            if (id != equipment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _equipmentsRepository.UpdateAsync(equipment);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _equipmentsRepository.ExistAsync(id))
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

            return View(equipment);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _equipmentsRepository.GetByIdAsync(id.Value);
               
            if (equipment == null)
            {
                return NotFound();
            }

            return View(equipment);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var equipment = await _equipmentsRepository.GetByIdAsync(id);

            if (equipment != null)
            {
                try
                {
                    await _equipmentsRepository.DeleteAsync(equipment);

                    return RedirectToAction("Index");
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
                    {
                        ViewBag.ErrorTitle = $"{equipment.Name} is probably being used!!!";
                        ViewBag.ErrorMessage = $"{equipment.Name} can't be deleted because there are gym equipments that use it <br/>" +
                        $"First try deleting all the gym equipments that are using it," +
                        $" and delete it again";
                    }
                }

                return View("Error");
            }

            return NotFound();
        }        
    }
}
