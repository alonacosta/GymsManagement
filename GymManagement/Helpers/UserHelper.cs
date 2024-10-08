﻿using GymManagement.Data;
using GymManagement.Data.Entities;
using GymManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Helpers
{
    public class UserHelper : IUserHelper
        {
            private readonly DataContext _context;
            private readonly UserManager<User> _userManager;
            private readonly SignInManager<User> _signInManager;
            private readonly RoleManager<IdentityRole> _roleManager;

        public UserHelper(
            DataContext context,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task AddUsertoRole(User user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task CheckRoleAsync(string roleName)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExists) 
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName,
                });
            }
        }

        public ICollection<UserViewModel> GetAllUsers()
        {
            var userList = _context.Users.ToList();
            ICollection<UserViewModel> users = userList.Cast<User>().Select(item => new UserViewModel
            {
                Id = item.Id,
                Name = item.FullName,
                Email = item.Email,
                Role = _userManager.GetRolesAsync(item).Result.FirstOrDefault(),
            }).ToList();
            return users;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<User> GetUserById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<bool> IsUserInRoleAsync(User user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        public Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            return _signInManager.PasswordSignInAsync(
                model.Username,
                model.Password,
                model.RememberMe,
                false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public string GetUserRole(User user)
        {
            return _userManager.GetRolesAsync(user).Result.FirstOrDefault();
        }

        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task CreateUserEntity(User user, string roleName, int gymId)
        {
            if (roleName == "Client")
            {
                Client newClient = new Client 
                {
                    User = user,
                };
                await _context.Set<Client>().AddAsync(newClient);
                await AddClientToGymAsync(gymId, newClient);
            }
            if (roleName == "Employee")
            {
                Employee newEmployee = new Employee
                {
                    User = user,
                };
                await _context.Set<Employee>().AddAsync(newEmployee);
                await AddEmployeeToGymAsync(gymId, newEmployee);
            }
            await SaveAllAsync();
        }

        private async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task AddClientToGymAsync(int gymId, Client client)
        {
            var gym = await _context.Gyms
                .Include(g => g.Clients)
                .FirstOrDefaultAsync(g => g.Id == gymId);

            if (gym != null)
            {
                gym.Clients.Add(client);

                await _context.SaveChangesAsync();
            }
        }

        public async Task AddEmployeeToGymAsync(int gymId, Employee employee)
        {
            var gym = await _context.Gyms
                .Include(g => g.Employees)
                .FirstOrDefaultAsync(g => g.Id == gymId);

            if (gym != null)
            {
                gym.Employees.Add(employee);

                await _context.SaveChangesAsync();
            }
        }
    }
}
