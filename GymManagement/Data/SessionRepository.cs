namespace GymManagement.Data
{
    using GymManagement.Data.Entities;

    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly DataContext _context;

        public SessionRepository(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}
