namespace GymManagement.Data
{
    using GymManagement.Data.Entities;
    using Microsoft.EntityFrameworkCore;

    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly DataContext _context;

        public SessionRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task UpdateSessionWithAppointmentsAsync(int id, Session session)
        {
           var existingSession = await _context.Sessions
                .Include(s => s.Appointments)
                .Where(s => s.Id == id)
                .FirstOrDefaultAsync();

            if (existingSession == null) { return; }

            var appointments = await _context.Appointments
                .Include(a => a.Session)
                .Include(a => a.Client)
                .Where(a => a.Session.Id == id).ToListAsync();

            existingSession.Name = session.Name;
            existingSession.StartSession = session.StartSession;
            existingSession.EndSession = session.EndSession;
            existingSession.Capacity = session.Capacity;
            existingSession.IsGroup = session.IsGroup;
            existingSession.IsOnline = session.IsOnline;
            existingSession.Gym = session.Gym;
            existingSession.ImageId = session.ImageId;

            //foreach (var existingAppointment in existingSession.Appointments)
            //{
            //    var appointmentToUpdate = session.Appointments.FirstOrDefault(a => a.Id == existingAppointment.Id);
            //    if (appointmentToUpdate != null)
            //    {
            //        _context.Entry(existingAppointment).CurrentValues.SetValues(appointmentToUpdate);
            //    }
            //}
            //var appointmentsToUpdate = await _context.Appointments
            //    .Include(a => a.Session)
            //    .Include(a => a.Client)
            //    .Where(a => a.Session.Id == id)
            //    .ToListAsync();

            var appointmentsTempToUpdate = await _context.AppointmentsTemp
                     .Where(at => at.StartSession == session.StartSession
                      && at.EndSession == session.EndSession
                      && at.Name == session.Name) 
                     .ToListAsync();

            foreach (var appointmentTemp in appointmentsTempToUpdate)
            {
                appointmentTemp.Name = session.Name; 
                appointmentTemp.StartSession = session.StartSession;
                appointmentTemp.EndSession = session.EndSession;
                appointmentTemp.RemainingCapacity = session.RemainingCapacity; 
            }

            await _context.SaveChangesAsync();
        }

        //public async Task<Session> GetSessionByIdAsync(int id)
        //{
        //    return await _context.Sessions
        //        .Include(s => s.Appointments)
        //        .Where(s => s.Id == id)
        //        .FirstOrDefaultAsync();    
        //}
    }
}
