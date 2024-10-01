﻿namespace GymManagement.Helpers
{
    using GymManagement.Data.Entities;
    using GymManagement.Models;
    using Microsoft.AspNetCore.Identity;

    public interface IUserHelper
    {
        Task<User> GetUserByEmailAsync(string email);

        Task<IdentityResult> AddUserAsync(User user, string password);

        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task<User> GetUserById (string id);

        Task LogoutAsync();

        Task CheckRoleAsync(string roleName);

        Task<bool> IsUserInRoleAsync(User user, string roleName);

        Task AddUsertoRole(User user, string roleName);

        public ICollection<UserViewModel> GetAllUsers();

        string GetUserRole(User user);

        Task<IdentityResult> UpdateUserAsync(User user);
    }
}
