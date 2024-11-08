﻿namespace GymManagement.Data
{
    using GymManagement.Data.Entities;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<Equipment> Equipments { get; set; }

        public DbSet<GymEquipment> GymEquipments { get; set; }

        public DbSet<Discussion> Discussions { get; set; }

        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<AppointmentTemp> AppointmentsTemp { get; set; }

        public DbSet<Session> Sessions { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<Gym> Gyms { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Employee> Employees { get; set; }
        
        public DbSet<Post> Posts { get; set; }

        public DbSet<GymSession> GymSessions { get; set; }

        public DbSet<FreeAppointment> FreeAppointments { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override async void OnModelCreating(ModelBuilder modelBuilder)
        {
            var cascadeFKs = modelBuilder.Model
                .GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.NoAction;
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
