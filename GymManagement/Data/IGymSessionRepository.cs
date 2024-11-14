using GymManagement.Data.Entities;
using GymManagement.Models;

namespace GymManagement.Data
{
    public interface IGymSessionRepository : IGenericRepository<GymSession>
    {
        IQueryable GetGymSessions(int id, DateTime? startTime);
        IQueryable GetGroupGymSessions(int gymId, DateTime? startTime);
        IQueryable GetOnlineGymSessions(int gymId, DateTime? startTime);
        IQueryable GetGroupnOnlineGymSessions(int gymId, DateTime? startTime);        
        IQueryable GetGymSessions(int id);
        IQueryable<GymSession> GetGymSessionsById(int gymId);
        Task<GymSession> GetGymSessionByIdAsync(int id);
        Task UpdateSessionWithAppointmentsAsync(int id, GymSession gymSession);
        Task<bool> IsExistsRatingAsync(string userId, int gymSessionId);
        Task CreateRatingAsync(Rating rating);
        Task UpdateRatingAsync(Rating rating);
        Task<Rating> GetExistingRatingAsync(string userId, int gymSessionId);
        Task<int?> GetExistingRatingIdAsync(string userId, int gymSessionId);
        Task<List<GymSessionRatingViewModel>> GetGymSessionsWithAverageRatingAsync(int gymId);
    }
}
