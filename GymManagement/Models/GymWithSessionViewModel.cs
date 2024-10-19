using GymManagement.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagement.Models
{
    public class GymWithSessionViewModel : GymSession
    {       
        public IEnumerable<SelectListItem>? Sessions { get; set; }
    }
}
