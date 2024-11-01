using GymManagement.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagement.Data
{
    public class PositionRepository : GenericRepository<Position>, IPositionRepository
    {
        private readonly DataContext _context;

        public PositionRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> GetComboPositions()
        {
            var list = _context.Positions.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id.ToString(),
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a position...)",
                Value = "0"
            });

            return list;
        }
    }
}
