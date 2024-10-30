using GymManagement.Data;
using GymManagement.Data.Entities;
using GymManagement.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Controllers
{
    public class SubscriptionsController : Controller
    {
        private readonly ISubscriptionRepository _subscriptionRepository;

        public SubscriptionsController(ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        //// GET: Subscriptions
        //public IActionResult Index()
        //{
        //    return View(_subscriptionRepository.GetAll());
        //}

        // GET: Subscriptions/IndexForAdmin
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View(_subscriptionRepository.GetAll());
        }


        // GET: Subscriptions/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Subscriptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Subscription subscription)
        {
            if (ModelState.IsValid) 
            { 
                await _subscriptionRepository.CreateAsync(subscription);
                return RedirectToAction(nameof(Index));
            }
            return View(subscription);
        }

        // GET: Subscriptions/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subscription = await _subscriptionRepository.GetByIdAsync(id.Value);
            if (subscription == null)
            {
                return NotFound();
            }

            return View(subscription);  
        }

        // POST: Subscriptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Subscription subscription)
        {
            if (ModelState.IsValid) 
            { 
                await _subscriptionRepository.UpdateAsync(subscription); 
                return RedirectToAction(nameof(Index));
            }
            return View(subscription);
        }

        // GET: Subscriptions/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subscription = await _subscriptionRepository.GetByIdAsync(id.Value);

            if (subscription == null)
            {
                return NotFound();
            }
            return View(subscription);
        }

        // POST: subscriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subscription = await _subscriptionRepository.GetByIdAsync(id);

            await _subscriptionRepository.DeleteAsync(subscription);
            return RedirectToAction(nameof(Index));
        }
    }
}
