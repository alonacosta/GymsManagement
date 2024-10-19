using GymManagement.Data.Entities;
using GymManagement.Models;

namespace GymManagement.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public Gym ToGym(GymViewModel model, Guid imageId, bool isNew)
        {
            return new Gym
            {
                Id = isNew ? 0 : model.Id,
                Name = model.Name,
                Address = model.Address,
                CityId = model.CityId, 
                City = model.City,                
                ImageId = imageId,
            };
        }

        public GymViewModel ToGymViewModel(Gym gym)
        {
            return new GymViewModel
            {
                Id = gym.Id,
                Name = gym.Name,
                Address = gym.Address,
                CityId = gym.CityId,   
                City = gym.City,                 
                ImageId = gym.ImageId,               

            };
        }

        public Session ToSession(SessionViewModel model, Guid imageId, bool isNew) 
        {
            return new Session
            {
                Id = isNew ? 0 : model.Id,
                Name = model.Name,
                StartSession = model.StartSession,
                EndSession = model.EndSession,
                Capacity = model.Capacity,
                IsOnline = model.IsOnline,
                IsGroup = model.IsGroup,
                ImageId = imageId,
            };        
        }

        public SessionViewModel ToSessionViewModel(Session session) 
        {
            return new SessionViewModel
            {
                Id = session.Id,
                Name = session.Name,
                StartSession = session.StartSession,
                EndSession = session.EndSession,
                Capacity = session.Capacity,
                IsOnline = session.IsOnline,
                IsGroup = session.IsGroup,
                ImageId = session.ImageId,
            };
        }

        public GymSession ToGymSession(GymWithSessionViewModel model, bool isNew)
        {
            return new GymSession
            {
                Id = isNew ? 0 : model.Id,
                SessionId = model.SessionId,
                GymId = model.GymId,               
                StartSession = model.StartSession,
                EndSession = model.EndSession,
                Capacity= model.Capacity,
            };
        }

        public GymWithSessionViewModel ToGymWithSessionViewModel(GymSession gymSession)
        {
            return new GymWithSessionViewModel
            {
                Id = gymSession.Id,
                SessionId = gymSession.SessionId,
                GymId = gymSession.GymId,
                StartSession = gymSession.StartSession,
                EndSession = gymSession.EndSession,
                Capacity = gymSession.Capacity,
              
            };
        }
    }
}
