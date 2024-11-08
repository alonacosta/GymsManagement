namespace GymManagement.Data
{
    using GymManagement.Data.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface IEquipmentsRepository : IGenericRepository<Equipment>
    {
        IEnumerable<SelectListItem> GetComboEquipments();

        Task<Equipment> GetEquipmentByGymEquipmentIdAsync(int gymEquipmentId);
    }
}
