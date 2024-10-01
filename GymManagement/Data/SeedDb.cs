
using GymManagement.Data.Entities;
using GymManagement.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context,
            IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        internal async Task InitializeAsync()
        {
            await _context.Database.MigrateAsync();
          
            var user = await _userHelper.GetUserByEmailAsync("cet87.adm@gmail.com");

            if (user == null)
            {
                user = new User
                {
                    FirstName = "Admin",
                    LastName = "Cet87",
                    Email = "cet87.adm@gmail.com",
                    UserName = "cet87.adm@gmail.com",
                    PhoneNumber = "212343555"
                };

                var result = await _userHelper.AddUserAsync(user, "123456");

                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create user in seeder");
                }                
            }

            var gymsL = new List<Gym>();
            var gymsP = new List<Gym>();
           
            if (!_context.Countries.Any())
            {

               gymsL.Add(new Gym { Name = "Stong and Healthy", Address = "Rua Marques de Pombal" });
               gymsL.Add(new Gym { Name = "Stong and Healthy", Address = "Rua de Ouro" });

               gymsP.Add(new Gym { Name = "Stong and Healthy", Address = "Avenida dos Aliados" });
               gymsP.Add(new Gym { Name = "Stong and Healthy", Address = "Rua so Estádio" });

               var cities = new List<City>();

               cities.Add(new City { Name = "Lisbon", Gyms = gymsL });
               cities.Add(new City { Name = "Porto", Gyms = gymsP });

                _context.Countries.Add(new Country
                {
                    Cities = cities,
                    Name = "Portugal"
                });

                await _context.SaveChangesAsync();
            }            
        }
    }
}
