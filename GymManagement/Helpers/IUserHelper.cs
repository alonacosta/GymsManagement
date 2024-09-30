using GymManagement.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace GymManagement.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<IdentityResult> AddUserAsync(User user, string password);
    }
}
