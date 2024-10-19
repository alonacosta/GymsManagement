namespace GymManagement.Helpers
{
    using GymManagement.Data.Entities;
    using GymManagement.Models;

    public interface IConverterHelper
    {
        Gym ToGym(GymViewModel model, Guid imageId, bool isNew);

        GymViewModel ToGymViewModel(Gym gym);

        Session ToSession(SessionViewModel model, Guid imageId, bool isNew);

        SessionViewModel ToSessionViewModel(Session session);
        GymSession ToGymSession(GymWithSessionViewModel model, bool isNew);
        GymWithSessionViewModel ToGymWithSessionViewModel(GymSession gymSession);
    }
}
