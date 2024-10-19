using GymManagement.Data.Entities;

namespace GymManagement.Data
{
    public interface IGymSessionRepository : IGenericRepository<GymSession>
    {
        IQueryable GetGymSessions(int id);
        Task<GymSession> GetGymSessionByIdAsync(int id);
    }
}
