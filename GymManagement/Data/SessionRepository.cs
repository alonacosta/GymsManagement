namespace GymManagement.Data
{
    using GymManagement.Data.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly DataContext _context;

        public SessionRepository(DataContext context) : base(context)
        {
            _context = context;
        }  

        public IEnumerable<SelectListItem> GetComboSessions()
        {   
            var list = _context.Sessions.Select(s => new SelectListItem
           {
                Text = s.Name,
                    Value = s.Id.ToString()
                }).OrderBy(l => l.Text).ToList();

                list.Insert(0, new SelectListItem
                {
                    Text = "(Select a session...)",
                    Value = "0"
                });
                 return list;
        }
    }
}
