﻿using GymManagement.Data.Entities;

namespace GymManagement.Data
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        Task<IQueryable<Appointment>> GetAppointmentsAsync(string userName);
        IQueryable<AppointmentTemp> GetAppointmentsTemp();
        Task<Client> GetClientByUserIdAsync(string id);
        Task<Employee> GetEmployeeByUserIdAsync(string id);
        Task AddAppointmentTempAsync(AppointmentTemp appointmentTemp);
        Task<bool> IsClientHasAppointmentAsync(int clientId, DateTime startSession, string sessionName, int sessionId);
        Task<bool> ConfirmBookingAsync(int clientId, string nameSession);
        Task<bool> CancelBookingTempAsync(int id);

        Task<Appointment> GetAppointmentByIdAsync(int id);
        Task<AppointmentTemp> GetAppointmentTempByIdAsync(int id);
        Task<bool> ConfirmBookingAllAsync();
        Task<bool> CancelBookingAsync(int id);
    }
}
