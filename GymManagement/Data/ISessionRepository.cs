namespace GymManagement.Data
{
    using GymManagement.Data.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface ISessionRepository : IGenericRepository<Session>
    {
        Task UpdateSessionWithAppointmentsAsync(int id, Session session);
        IEnumerable<SelectListItem> GetComboSessions();
    }
}
