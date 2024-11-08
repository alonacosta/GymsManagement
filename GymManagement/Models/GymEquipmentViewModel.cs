namespace GymManagement.Models
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class GymEquipmentViewModel
    {
        public int Id { get; set; }

        public int GymId { get; set; }

        [Display(Name = "Equipments")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select an equipment")]
        public int EquipmentId { get; set; }

        public IEnumerable<SelectListItem>? Equipments { get; set; }

        public bool IsActive { get; set; }

        [Display(Name ="Equipment Name")]
        public string? EquipmentName { get; set; }
    }
}
