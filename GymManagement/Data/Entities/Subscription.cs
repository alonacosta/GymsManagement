using System.ComponentModel.DataAnnotations;

namespace GymManagement.Data.Entities
{
    public class Subscription: IEntity
    { 
        public int Id { get; set; }
        public string Name { get; set; }

        [Display(Name = "Items (Write items separated by coma)")]
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Price { get; set; }
    }
}
