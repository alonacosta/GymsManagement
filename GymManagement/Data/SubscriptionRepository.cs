using GymManagement.Data.Entities;

namespace GymManagement.Data
{
    public class SubscriptionRepository : GenericRepository<Subscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(DataContext context) : base(context)
        {
        }

      
    }
}
