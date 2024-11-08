namespace GymManagement.Data.Entities
{
    public class Equipment : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<GymEquipment>? GymEquipments { get; set; }
    }
}
