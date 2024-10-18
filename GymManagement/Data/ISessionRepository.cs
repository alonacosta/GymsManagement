namespace GymManagement.Data
{
    using GymManagement.Data.Entities;

    public interface ISessionRepository : IGenericRepository<Session>
    {
        Task UpdateSessionWithAppointmentsAsync(int id, Session session);
    }
}
