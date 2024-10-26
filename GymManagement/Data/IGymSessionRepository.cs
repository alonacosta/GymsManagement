using GymManagement.Data.Entities;

namespace GymManagement.Data
{
    public interface IGymSessionRepository : IGenericRepository<GymSession>
    {
        IQueryable GetGymSessions(int id);

        IQueryable GetGroupGymSessions(int gymId);

        public IQueryable GetOnlineGymSessions(int gymId);

        public IQueryable GetGroupnOnlineGymSessions(int gymId);
        
        Task<GymSession> GetGymSessionByIdAsync(int id);
        Task UpdateSessionWithAppointmentsAsync(int id, GymSession gymSession);
    }
}
