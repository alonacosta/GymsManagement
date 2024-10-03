using GymManagement.Data;
using GymManagement.Data.Entities;
using GymManagement.Helpers;
using GymManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly ICountryRepository _countryRepository;
        private readonly IGymRepository _gymRepository;

        public AccountController(IUserHelper userHelper,
            ICountryRepository countryRepository,
            IGymRepository gymRepository)
        {
            _userHelper = userHelper;
            _countryRepository = countryRepository;
            _gymRepository = gymRepository;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(Request.Query["ReturnUrl"].First());
                    }
                    return RedirectToAction("Index", "Home");
                }
            }

            this.ModelState.AddModelError(string.Empty, "Failed to login.");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult UserManagementIndex() 
        {
            var model = _userHelper.GetAllUsers();
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Register(string roleName)
        {
            var model = new RegisterUserViewModel
            {
                RoleName = roleName,
                Countries = _countryRepository.GetComboCountries(),
                Cities = _countryRepository.GetComboCities(0),
                Gyms = _gymRepository.GetComboGyms(0)
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            var user = await _userHelper.GetUserByEmailAsync(model.Username);
            if (user == null)
            {
                user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.Username,
                    Email = model.Username,
                    PhoneNumber = model.PhoneNumber
                };

            }
            var result = await _userHelper.AddUserAsync(user, model.Password);

            if (result != IdentityResult.Success)
            {
                ModelState.AddModelError(string.Empty, "The user could not be registered.");
                return View(model);
            }

            await _userHelper.CreateUserEntity(user, model.RoleName, model.GymId);

            await _userHelper.AddUsertoRole(user, model.RoleName);


            ViewBag.Message = "The user has been created succesfully.";

            return View(model);
        }

        /*[HttpPost]
        public async Task<IActionResult> RegisterClient(RegisterUserViewModel model)
        {
            var user = await _userHelper.GetUserByEmailAsync(model.Username);
            if (user == null)
            {
                user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.Username,
                    Email = model.Username,
                    PhoneNumber = model.PhoneNumber
                };
            }
            var result = await _userHelper.AddUserAsync(user, model.Password);

            if (result != IdentityResult.Success)
            {
                ModelState.AddModelError(string.Empty, "The user could not be registered.");
                return View(model);
            }

            await _userHelper.CreateUserEntity(user, "Client");

            await _userHelper.AddUsertoRole(user, "Client");

            ViewBag.Message = "The user has been created succesfully.";

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult RegisterEmployee()
        {
            var model = new RegisterUserViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterEmployee(RegisterUserViewModel model)
        {
            var user = await _userHelper.GetUserByEmailAsync(model.Username);
            if (user == null)
            {
                user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.Username,
                    Email = model.Username,
                    PhoneNumber = model.PhoneNumber
                };
            }
            var result = await _userHelper.AddUserAsync(user, model.Password);

            if (result != IdentityResult.Success)
            {
                ModelState.AddModelError(string.Empty, "The user could not be registered.");
                return View(model);
            }

            await _userHelper.CreateUserEntity(user, "Employee");

            await _userHelper.AddUsertoRole(user, "Employee");

            ViewBag.Message = "The user has been created succesfully.";

            return View(model);
        }*/

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(string id)
        {
            var user = await _userHelper.GetUserById(id);
            UserViewModel model = new UserViewModel
            {
                Id = user.Id,
                Name = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = _userHelper.GetUserRole(user)
            };
            return View(model);
        }

        [Authorize(Roles="Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userHelper.GetUserById(id);
            var model = new EditUserViewModel();
            if (user != null)
            {
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.Username = user.UserName;
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            var user = await _userHelper.GetUserByEmailAsync(model.Username);
            if (user != null)
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;

                var response = await _userHelper.UpdateUserAsync(user);

                if (response.Succeeded)
                {
                    ViewBag.Message = "User updated!";
                }
                else 
                {
                    ModelState.AddModelError(string.Empty, response.Errors.FirstOrDefault().Description);
                }
            }
            return View(model);
        }
        
        public async Task<IActionResult> ChangeUser()
        {
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            var model = new ChangeUserViewModel();
            if (user != null)
            {
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.PhoneNumber = user.PhoneNumber;
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUser(ChangeUserViewModel model)
        {
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            if (user != null)
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.PhoneNumber = model.PhoneNumber;
                var response = await _userHelper.UpdateUserAsync(user);
                if (response.Succeeded) 
                {
                    ViewBag.Message = "User updated!";
                }
                else
                {
                    ModelState.AddModelError(string.Empty, response.Errors.FirstOrDefault().Description);
                }
            }
            return View(model);
        }

        [HttpPost]
        [Route("Account/GetCitiesAsync")]
        public async Task<IActionResult> GetCitiesAsync(int countryId)
        {
            var country = await _countryRepository.GetCountriesWithCitiesAsync(countryId);
            return Json(country.Cities.OrderBy(c => c.Name));
        }

        [HttpPost]
        [Route("Account/GetGymsAsync")]
        public async Task<IActionResult> GetGymsAsync(int cityId)
        {
            var gyms = await _gymRepository.GetGymsByCityId(cityId);
            return Json(gyms.OrderBy(g => g.Name));
        }
    }
}
