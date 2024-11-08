namespace GymManagement.Data.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class GymEquipment : IEntity
    {
        public int Id { get; set; }

        public Gym Gym { get; set; }

        public int GymId { get; set; }

        public Equipment? Equipment { get; set; }

        public int EquipmentId { get; set; }

        [Display(Name ="Is Active")]
        public bool IsActive { get; set; }

        [Display(Name ="Equipment Name")]
        public string EquipmentName => Equipment.Name;
    }
}
