using GymManagement.Data.Entities;
using GymManagement.Models;

namespace GymManagement.Helpers
{
    public interface IConverterHelper
    {
        Gym ToGym(GymViewModel model, Guid imageId, bool isNew);

        GymViewModel ToGymViewModel(Gym gym);
    }
}
