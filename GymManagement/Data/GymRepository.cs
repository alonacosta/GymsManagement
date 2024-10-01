using GymManagement.Data.Entities;

namespace GymManagement.Data
{
    public class GymRepository : GenericRepository<Gym>, IGymRepository
    {
        public GymRepository(DataContext context) : base(context)
        {
        }
    }
}
