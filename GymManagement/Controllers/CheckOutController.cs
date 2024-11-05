using GymManagement.Data;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace GymManagement.Controllers
{
    public class CheckOutController : Controller
    {
        private readonly ISubscriptionRepository _subscriptionRepository;

        public CheckOutController(ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        public IActionResult Index()
        {
            var subscriptions = _subscriptionRepository.GetAll();

            return View(subscriptions);
        }

        public async Task<IActionResult> CheckOut(int? id)
        {
            if (id == null) { return NotFound(); }

            var subscription = await _subscriptionRepository.GetByIdAsync(id.Value);

            //var domain = "https://localhost:44346/";
            var domain = "https://gymmanagementsystem.azurewebsites.net/";

            var options = new Stripe.Checkout.SessionCreateOptions
            {
                SuccessUrl = domain + $"CheckOut/OrderConfirmation",
                CancelUrl = domain + $"CheckOut/CancelCheckOut",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };           

            var sessionListItem = new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)(subscription.Price * 100),
                    Currency = "eur",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = $"Plan - {subscription.Name}",
                    }
                },
                Quantity = 1,
            };
            options.LineItems.Add(sessionListItem);


            var service = new Stripe.Checkout.SessionService();
            Stripe.Checkout.Session session = service.Create(options);

            TempData["Session"] = session.Id;

            Response.Headers.Add("Location", session.Url);

            return new StatusCodeResult(303);            
        }

        public IActionResult OrderConfirmation()
        {
            var service = new Stripe.Checkout.SessionService();
            Stripe.Checkout.Session session = service.Get(TempData["Session"].ToString());

            if (session.PaymentStatus == "paid")
            {
                var transition = session.PaymentIntentId.ToString();
                return View("Success");
            }
            return View("CancelCheckOut");
        }

        public IActionResult Success()
        {
            return View();
        }

        public IActionResult CancelCheckOut()
        {
            return View();
        }
    }
}
