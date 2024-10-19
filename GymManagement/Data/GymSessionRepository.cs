using GymManagement.Data.Entities;
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

        public IQueryable GetGymSessions(int gymId)
        {
            return _context.GymSessions
                .Include(gs => gs.Session)
                .Include(gs => gs.Gym)
                .Where(gs => gs.Gym.Id == gymId)
                .OrderBy(gs => gs.StartSession);
        }

        public async Task<GymSession> GetGymSessionByIdAsync(int id)
        {
            return await _context.GymSessions
                .Include(gs => gs.Session)
                .Include(gs => gs.Gym)
                .Where(gs => gs.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}
