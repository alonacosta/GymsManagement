namespace GymManagement.Helpers
{
    using GymManagement.Data.Entities;
    using Microsoft.AspNetCore.Identity;

    public interface IUserHelper
    {
        Task<User> GetUserByEmailAsync(string email);

        Task<IdentityResult> AddUserAsync(User user, string password);
    }
}
