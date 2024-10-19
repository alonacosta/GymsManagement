using GymManagement.Data.Entities;
using GymManagement.Helpers;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Data
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public AppointmentRepository(DataContext context,
            IUserHelper userHelper) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task<IQueryable<Appointment>> GetAppointmentsAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);

            if (user == null) 
            { 
                return null; 
            }

            if(await _userHelper.IsUserInRoleAsync(user, "Admin")) {
                return _context.Appointments
                    .Include(a => a.Client)
                    .Include(a => a.Session)                   
                    .OrderByDescending(a => a.Session.StartSession);                   
            }

            return _context.Appointments
                .Include(a => a.Session)
                .Include(a => a.Client)                
                .Where(a => a.Client.User.Id == user.Id)
                .OrderByDescending(a => a.Session.StartSession);
        }

        public IQueryable<AppointmentTemp> GetAppointmentsTemp()
        {
            return _context.AppointmentsTemp
                .Include(a => a.Client)
                .ThenInclude(a=> a.User)
                .OrderBy(a => a.Id);
        }

        public async Task<Client> GetClientByUserIdAsync(string id)
        {
            return await _context.Clients
                .Where(c => c.User.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task AddAppointmentTempAsync(AppointmentTemp appointmentTemp)
        {
            if (appointmentTemp == null) { return; }

            _context.AppointmentsTemp.Add(appointmentTemp);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsClientHasAppointmentAsync(int clientId, string sessionName, int sessionId)
        {
            var clientHasAppointmentTemp = await _context.AppointmentsTemp
                .Where(at => at.Client.Id == clientId && at.Name == sessionName).FirstOrDefaultAsync();

            var clientHasAppointment = await _context.Appointments
                .Where(a => a.Client.Id == clientId && a.Session.Id == sessionId).FirstOrDefaultAsync();

            if (clientHasAppointment != null || clientHasAppointmentTemp != null) 
            { 
                return true;    
            }
            return false;
        }

        public async Task<bool> ConfirmBookingAsync(int clientId, string nameSession)
        {
            var appointmentTemp = await _context.AppointmentsTemp
                .Include(at => at.Client)
                .Where(at => at.Client.Id == clientId && at.Name == nameSession)
                .FirstOrDefaultAsync();

            if (appointmentTemp == null) 
            {
                return false;
            }

            var client = await _context.Clients
                .Where(c => c.Id == clientId).FirstOrDefaultAsync();
            if (client == null)
            {
                return false;
            }

            var session = await _context.Sessions
                .Where(s => s.Name == nameSession).FirstOrDefaultAsync();
            if (session == null)
            {
                return false;
            }

            var appointment = new Appointment
            {
                Client = client,
                Session = session,
            };

            await CreateAsync(appointment);
            _context.AppointmentsTemp.Remove(appointmentTemp);
            await _context.SaveChangesAsync();

            return true;    
        }

        public async Task<bool> CancelBookingAsync(int id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Client)
                .Include(a => a.Session)
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();

            if (appointment == null)
            {
                return false;
            }

            var sessionId = appointment.Id;
            

          var session = await _context.Sessions                
                .Where(s => s.Id == sessionId).FirstOrDefaultAsync();
            
            if (session == null) 
            {
                return false;
            }

            session.Capacity++;

            _context.Appointments.Remove(appointment);
            _context.Update(session);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<AppointmentTemp> GetAppointmentTempByIdAsync(int id)
        {
            return await _context.AppointmentsTemp
                .Include(at => at.Client)
                .Where(at => at.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> ConfirmBookingAllAsync()
        {
            var appointmentsTemp = await _context.AppointmentsTemp
                .Include(at => at.Client)
                .ToListAsync();

            if(appointmentsTemp.Count == 0 || appointmentsTemp == null)
            { 
                return false;
            }   

            var appointments = new List<Appointment>();

            foreach (var temp in appointmentsTemp) 
            {
                var session = await _context.Sessions
                    .Where(s => s.Name == temp.Name).FirstOrDefaultAsync();

                var appointment = new Appointment
                {
                    Session = session,
                    Client = temp.Client,
                };

                appointments.Add(appointment);
            }

           await _context.Appointments.AddRangeAsync(appointments);
            _context.AppointmentsTemp.RemoveRange(appointmentsTemp);
            await _context.SaveChangesAsync();

            return true;
          
        }
    }
}
