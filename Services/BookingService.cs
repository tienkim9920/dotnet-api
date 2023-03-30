using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ResortProjectAPI.ModelEF;
using ResortProjectAPI.IServices;
using Microsoft.EntityFrameworkCore;

namespace ResortProjectAPI.Services
{
    public class BookingService : IBookingService
    {
        private readonly ResortDBContext db;
        public BookingService(ResortDBContext context)
        {
            db = context;
        }
        public async Task<IEnumerable<Booking>> GetAll()
        {
            return await db.Bookings
                    .Include(b => b.Room)
                    .Include(b => b.Customer)
                    .Include(b => b.Voucher)
                    .Include(b => b.Services)
                    .ThenInclude(s => s.Service)
                    .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> BookingsOfCustomer(string id)
        {
            return await db.Bookings
                    .Include(b => b.Room)
                    .Include(b => b.Customer)
                    .Include(b => b.Voucher)
                    .Include(b => b.Services)
                    .ThenInclude(s => s.Service)
                    .Where(b => b.CustomerID == id)
                    .OrderByDescending(b => b.ID)
                    .ToListAsync();
        }

        public async Task<Booking> GetByID(int id)
        {
            return await db.Bookings
                    .Include(b => b.Room)
                    .Include(b => b.Customer)
                    .Include(b => b.Voucher)
                    .Include(b => b.Services)
                    .ThenInclude(s => s.Service)
                    .SingleOrDefaultAsync(b => b.ID == id);
        }

        public async Task<IEnumerable<Customer>> GetCustomerAvailable()
        {
            return await db.Customers
                    .Include(c => c.Bookings)
                    .Where(c => c.Bookings.All(b => b.Status == "checkout" || b.Status == "cancel" || b.Status == "payment"))
                    .ToListAsync();
        }

        public async Task<IEnumerable<Room>> GetRoomAvailable(DateTime from, DateTime to)
        {
            var list = await db.Rooms.Include(r => r.Bookings)
                    .Where(r => r.Bookings.Any(b =>
                    ((b.CheckinDate.Date <= from.Date && from.Date <= b.CheckinDate.Date) ||
                    (b.CheckoutDate.Date <= to.Date && to.Date <= b.CheckoutDate.Date)) &&
                    (b.Status != "cancel")))
                    .Select(r => r.ID)
                    .ToListAsync();
            return await db.Rooms.Include(r => r.Bookings)
                .Where(r => !list.Contains(r.ID)).ToListAsync();
        }
        public async Task<int> Create(Booking model)
        {
            db.Bookings.Add(model);
            return await db.SaveChangesAsync();
        }

        public async Task<int> Edit(Booking model)
        {
            Booking enti = await db.Bookings.FindAsync(model.ID);
            enti.CustomerID = model.CustomerID;
            enti.CheckinDate = model.CheckinDate;
            enti.CheckoutDate = model.CheckoutDate;
            enti.Adult = model.Adult;
            enti.Child = model.Child;
            enti.RoomID = model.RoomID;
            enti.VoucherCode = model.VoucherCode;
            return await db.SaveChangesAsync();
        }

        public async Task<int> Payment(int id, string code)
        {
            Booking enti = await db.Bookings.FindAsync(id);
            enti.VoucherCode = code;
            enti.Status = "payment";
            return await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<string>> GetServicesOfBill(int id)
        {
            return await db.BookingServices.Where(b => b.BookingID == id).Select(b => b.ServiceID).ToListAsync();
        }

        public async Task<int> AddSV(BookingServices model)
        {
            db.BookingServices.Add(model);
            return await db.SaveChangesAsync();
        }

        public async Task<int> RemoveSV(BookingServices model)
        {
            var enti = await db.BookingServices.Where(b => b.BookingID == model.BookingID && b.ServiceID == model.ServiceID).SingleOrDefaultAsync();
            db.BookingServices.Remove(enti);
            return await db.SaveChangesAsync();
        }

        public async Task<int> Remove(int id)
        {
            var booking = await db.Bookings.Where(b => b.ID == id).Include(b => b.Services).SingleOrDefaultAsync();
            foreach (var item in booking.Services.ToList())
            {
                db.BookingServices.Remove(item);
            }
            db.Bookings.Remove(booking);
            return await db.SaveChangesAsync();
        }

        public async Task<int> CheckBooking(int id, string status)
        {
            var booking = await db.Bookings.FindAsync(id);
            booking.Status = status;
            return await db.SaveChangesAsync();
        }
    }
}
