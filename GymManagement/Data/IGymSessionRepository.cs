using GymManagement.Data.Entities;

namespace GymManagement.Data
{
    public interface IGymSessionRepository : IGenericRepository<GymSession>
    {
        IQueryable GetGymSessions(int id);
        IQueryable<GymSession> GetGymSessionsById(int gymId);
        Task<GymSession> GetGymSessionByIdAsync(int id);
        Task UpdateSessionWithAppointmentsAsync(int id, GymSession gymSession);
    }
}
