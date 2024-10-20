using GymManagement.Data.Entities;

namespace GymManagement.Data
{
    public interface IFreeAppointmentRepository : IGenericRepository<FreeAppointment>
    {
        IQueryable GetAllNotCompleteFreeAppointments();
        IQueryable GetCompleteFreeAppointments();
        Task<bool> HasFreeApointment(string email);
    }
}
