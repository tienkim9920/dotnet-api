using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ResortProjectAPI.ModelEF;

namespace ResortProjectAPI.IServices
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> GetAll();

        Task<IEnumerable<Booking>> BookingsOfCustomer(string id);

        Task<Booking> GetByID(int id);

        Task<IEnumerable<Customer>> GetCustomerAvailable();

        Task<IEnumerable<Room>> GetRoomAvailable(DateTime from, DateTime to);

        Task<int> Create(Booking model);

        Task<int> Edit(Booking model);

        Task<IEnumerable<string>> GetServicesOfBill(int id);

        Task<int> AddSV(BookingServices model);

        Task<int> RemoveSV(BookingServices model);

        Task<int> Remove(int id);

        Task<int> CheckBooking(int id, string status);

        Task<int> Payment(int id, string code);
    }
}
