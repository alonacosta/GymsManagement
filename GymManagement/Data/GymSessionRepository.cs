using GymManagement.Data.Entities;
using GymManagement.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Data
{
    public class GymSessionRepository : GenericRepository<GymSession>, IGymSessionRepository
    {
        private readonly DataContext _context;

        public GymSessionRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable GetGymSessions(int gymId, DateTime? startTime)
        {
            if (startTime == null)
            {
                return _context.GymSessions
                .Include(gs => gs.Session)
                .Include(gs => gs.Gym)
                .Where(gs => gs.Gym.Id == gymId)
                .OrderByDescending(gs => gs.StartSession);
            }
            else
            {
                return _context.GymSessions
                .Include(gs => gs.Session)
                .Include(gs => gs.Gym)
                .Where(gs => gs.Gym.Id == gymId)
                .Where(gs => gs.StartSession > startTime)
                .OrderByDescending(gs => gs.StartSession);
            }
        }

        public IQueryable<GymSession> GetGymSessionsById(int gymId)
        {            
            return _context.GymSessions
            .Include(gs => gs.Session)
            .Include(gs => gs.Gym)
            .Where(gs => gs.Gym.Id == gymId)
            .OrderByDescending(gs => gs.StartSession);            
        }

        public IQueryable GetOnlineGymSessions(int gymId, DateTime? startTime)
        {
            if (startTime == null)
            {
                return _context.GymSessions
                .Include(gs => gs.Session)
                .Include(gs => gs.Gym)
                .Where(gs => gs.Gym.Id == gymId)
                .Where(gs => gs.Session.IsOnline == true)
                .OrderByDescending(gs => gs.StartSession);
            }
            else
            {
                return _context.GymSessions
                .Include(gs => gs.Session)
                .Include(gs => gs.Gym)
                .Where(gs => gs.Gym.Id == gymId)
                .Where(gs => gs.Session.IsOnline == true)
                .Where(gs => gs.StartSession > startTime)
                .OrderByDescending(gs => gs.StartSession);
            }
        } 

        public IQueryable GetGroupGymSessions(int gymId, DateTime? startTime)
        {
            if (startTime == null)
            {
                return _context.GymSessions
                .Include(gs => gs.Session)
                .Include(gs => gs.Gym)
                .Where(gs => gs.Gym.Id == gymId)
                .Where(gs => gs.Session.IsGroup == true)
                .OrderByDescending(gs => gs.StartSession);
            }
            else
            {
                return _context.GymSessions
                .Include(gs => gs.Session)
                .Include(gs => gs.Gym)
                .Where(gs => gs.Gym.Id == gymId)
                .Where(gs => gs.Session.IsGroup == true)
                .Where(gs => gs.StartSession > startTime)
                .OrderByDescending(gs => gs.StartSession);
            }
        } 

        public IQueryable GetGroupnOnlineGymSessions(int gymId, DateTime? startTime)
        {
            if (startTime == null)
            {
                return _context.GymSessions
                .Include(gs => gs.Session)
                .Include(gs => gs.Gym)
                .Where(gs => gs.Gym.Id == gymId)
                .Where(gs => gs.Session.IsGroup == true)
                .Where(gs => gs.Session.IsOnline == true)
                .OrderByDescending(gs => gs.StartSession);
            }
            else
            {
                return _context.GymSessions
                .Include(gs => gs.Session)
                .Include(gs => gs.Gym)
                .Where(gs => gs.Gym.Id == gymId)
                .Where(gs => gs.Session.IsGroup == true)
                .Where(gs => gs.Session.IsOnline == true)
                .Where(gs => gs.StartSession > startTime)
                .OrderByDescending(gs => gs.StartSession);
            }
        } 

        public async Task<GymSession> GetGymSessionByIdAsync(int id)
        {
            return await _context.GymSessions
                .Include(gs => gs.Session)
                .Include(gs => gs.Gym)
                .Where(gs => gs.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateSessionWithAppointmentsAsync(int id, GymSession gymSession)
        {
            var existingSession = await _context.GymSessions
                .Include(gs => gs.Session)
                .Include(gs => gs.Gym)
                 .Include(s => s.Appointments)
                 .Where(s => s.Id == id)
                 .FirstOrDefaultAsync();

            if (existingSession == null || existingSession.Session == null) { return; }

            var appointmentsTempToUpdate = await _context.AppointmentsTemp
                     .Where(at => at.StartSession == existingSession.StartSession
                      && at.EndSession == existingSession.EndSession
                      && at.Name == existingSession.Session.Name)
                     .ToListAsync();            

            existingSession.SessionId = gymSession.SessionId;
            existingSession.StartSession = gymSession.StartSession;
            existingSession.EndSession = gymSession.EndSession;
            existingSession.Capacity = gymSession.Capacity;            
            existingSession.GymId = gymSession.GymId;    

            foreach (var appointmentTemp in appointmentsTempToUpdate)
            {
                appointmentTemp.Name = existingSession.Session.Name;
                appointmentTemp.StartSession = existingSession.StartSession;
                appointmentTemp.EndSession = existingSession.EndSession;
                appointmentTemp.RemainingCapacity = existingSession.Capacity;
            }

            await _context.SaveChangesAsync();
        }

        public IQueryable GetGymSessions(int id)
        {
            return _context.GymSessions
               .Include(gs => gs.Session)
               .Include(gs => gs.Gym)
               .Where(gs => gs.Gym.Id == id)
               .OrderByDescending(gs => gs.StartSession);
        }

        public async Task<bool> IsExistsRatingAsync(string userId, int gymSessionId)
        {
            return await _context.Ratings.AnyAsync(r => r.UserId == userId && r.GymSessionId == gymSessionId);
        }

        public async Task CreateRatingAsync(Rating rating)
        {
            await _context.Ratings.AddAsync(rating);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRatingAsync(Rating rating)
        {
             _context.Ratings.Update(rating);
            await _context.SaveChangesAsync();
        }

        public async Task<Rating> GetExistingRatingAsync(string userId, int gymSessionId)
        {
            return await _context.Ratings
                .Where(r => r.UserId == userId && r.GymSessionId == gymSessionId)               
                .FirstOrDefaultAsync();
        }

        public async Task<int?> GetExistingRatingIdAsync(string userId, int gymSessionId)
        {
            return await _context.Ratings
                .Where(r => r.UserId == userId && r.GymSessionId == gymSessionId)
                .Select(r => (int?)r.Id)
                .FirstOrDefaultAsync();
        }


        public async Task<List<GymSessionRatingViewModel>> GetGymSessionsWithAverageRatingAsync(int gymId)
        {
            return await _context.Ratings     
                .Include(r => r.GymSession)
                .ThenInclude(gs => gs.Session)
                .ThenInclude(gs => gs.Gym)
                .Where(r => r.GymSession.Gym.Id == gymId)                
                .GroupBy(r => new { r.GymSession.Session.Name, r.GymSession.Gym.Id })
                .Select(g => new GymSessionRatingViewModel
                {                    
                    GymSessionName = g.Key.Name,
                    GymId = g.Key.Id,
                    AverageRating = g.Average(r => r.Rate),         
                })
                .ToListAsync();
        }
    }
}
