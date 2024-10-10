using GymManagement.Data.Entities;
using GymManagement.Models;
using Microsoft.AspNetCore.Identity;

namespace GymManagement.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserByEmailAsync(string email);

        Task<IdentityResult> AddUserAsync(User user, string password);

        Task CreateUserEntity(User user, string roleName, int gymId);

        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task<User> GetUserById (string id);

        Task LogoutAsync();

        Task CheckRoleAsync(string roleName);

        Task<bool> IsUserInRoleAsync(User user, string roleName);

        Task AddUsertoRole(User user, string roleName);

        public ICollection<UserViewModel> GetAllUsers();

        string GetUserRole(User user);

        Task<IdentityResult> UpdateUserAsync(User user);

        Task AddClientToGymAsync(int gymId, Client client);

        Task AddEmployeeToGymAsync(int gymId, Employee employee);

        //Task<Gym> GetUserGymAsync(User user);
    }
}
