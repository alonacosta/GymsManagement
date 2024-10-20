using GymManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Data
{
    public class FreeAppointmentRepository : GenericRepository<FreeAppointment>, IFreeAppointmentRepository
    {
        private readonly DataContext _context;

        public FreeAppointmentRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable GetAllNotCompleteFreeAppointments()
        {
            return _context.FreeAppointments
                .Where(fa => fa.IsComplete == false || fa.IsComplete == null)
                .OrderBy(fa => fa.Id);
        }

        public IQueryable GetCompleteFreeAppointments()
        {
            return _context.FreeAppointments
                .Where(fa => fa.IsComplete == true)
                .OrderBy(fa => fa.Id);
        }

        public async Task<bool> HasFreeApointment(string email)
        {
            var hasFreeAppointment = await _context.FreeAppointments
                .Where(fa => fa.Email == email)
                .FirstOrDefaultAsync();

            if(hasFreeAppointment == null) 
            {
                return false;
            }

            return true;
        }
    }
}
