﻿using GymManagement.Data.Entities;
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

        public IQueryable<GymSession> GetGymSessionsById(int gymId)
        {
            return _context.GymSessions
                .Include(gs => gs.Session)
                .Include(gs => gs.Gym)
                .Where(gs => gs.Gym.Id == gymId)
                .OrderBy(gs => gs.StartSession);
            }
            else
            {
                return _context.GymSessions
                .Include(gs => gs.Session)
                .Include(gs => gs.Gym)
                .Where(gs => gs.Gym.Id == gymId)
                .Where(gs => gs.StartSession > startTime)
                .OrderBy(gs => gs.StartSession);
            }

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
                .OrderBy(gs => gs.StartSession);
            }
            else
            {
                return _context.GymSessions
                .Include(gs => gs.Session)
                .Include(gs => gs.Gym)
                .Where(gs => gs.Gym.Id == gymId)
                .Where(gs => gs.Session.IsOnline == true)
                .Where(gs => gs.StartSession > startTime)
                .OrderBy(gs => gs.StartSession);
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
                .OrderBy(gs => gs.StartSession);
            }
            else
            {
                return _context.GymSessions
                .Include(gs => gs.Session)
                .Include(gs => gs.Gym)
                .Where(gs => gs.Gym.Id == gymId)
                .Where(gs => gs.Session.IsGroup == true)
                .Where(gs => gs.StartSession > startTime)
                .OrderBy(gs => gs.StartSession);
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
                .OrderBy(gs => gs.StartSession);
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
                .OrderBy(gs => gs.StartSession);
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
    }
}
