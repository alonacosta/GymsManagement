using GymManagement.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagement.Data
{
    public interface IPositionRepository : IGenericRepository<Position>
    {
        IEnumerable<SelectListItem> GetComboPositions();
    }
}
