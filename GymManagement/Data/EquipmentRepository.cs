using GymManagement.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Data
{
    public class EquipmentRepository : GenericRepository<Equipment>, IEquipmentsRepository
    {
        private readonly DataContext _context;

        public EquipmentRepository(DataContext context) : base(context) 
        {
            _context = context;    
        }

        public IEnumerable<SelectListItem> GetComboEquipments() 
        { 
            var list = _context.Equipments
                .Select(e => new SelectListItem 
                { 
                    Text = e.Name,
                    Value = e.Id.ToString(),

                }).OrderBy(l => l.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select an equipment ...)",
                Value = "0",
            });

            return list;
        }

        public async Task<Equipment> GetEquipmentByGymEquipmentIdAsync(int gymEquipmentId) 
        { 
            var gymEquipment = await _context.GymEquipments
                .Include(g => g.Equipment)
                .FirstOrDefaultAsync(g => g.Id == gymEquipmentId);

            return gymEquipment.Equipment;
        }
    }
}
