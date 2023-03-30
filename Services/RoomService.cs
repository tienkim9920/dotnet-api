using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ResortProjectAPI.IServices;
using ResortProjectAPI.ModelEF;

namespace ResortProjectAPI.Services
{
    public class RoomService : IRoomService
    {
        private readonly ResortDBContext db;

        public RoomService(ResortDBContext context)
        {
            db = context;
        }
        public async Task<int> Create(Room room)
        {
            room.ID = room.ID.Trim().ToUpper();
            db.Rooms.Add(room);
            return await db.SaveChangesAsync();
        }

        public async Task<int> Delete(string id)
        {
            Room room = await db.Rooms
                    .Include(r => r.RoomType)
                    .Include(r => r.Images)
                    .Include(r => r.Supplies)
                    .ThenInclude(sr => sr.Supply)
                    .SingleOrDefaultAsync(r => r.ID == id);
            foreach (var img in room.Images.ToList())
            {
                db.Images.Remove(img);
            }
            foreach (var s in room.Supplies.ToList())
            {
                Supply supply = await db.Supplies.FindAsync(s.SupplyID);
                supply.Total += s.Count;
                db.SuppliesForRooms.Remove(s);
            }
            db.Rooms.Remove(room);
            return await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Room>> GetAll()
        {
            return await db.Rooms
            .Include(r => r.RoomType)
            .Include(r => r.Images)
            .Include(r => r.Supplies)
            //.ThenInclude(sr => sr.Supply)
            .ToListAsync();
        }

        public async Task<Room> GetByID(string id)
        {
            return await db.Rooms
                    .Include(r => r.RoomType)
                    .Include(r => r.Images)
                    .Include(r => r.Supplies).SingleOrDefaultAsync(r => r.ID == id);
        }

        public async Task<int> Update(Room room)
        {
            Room enti = await db.Rooms.SingleOrDefaultAsync(s => s.ID.ToLower() == room.ID.Trim().ToLower());
            if (enti != null)
            {
                enti.Name = room.Name;
                enti.Price = room.Price;
                enti.Description = room.Description;
                enti.TypeID = room.TypeID;
                enti.Status = room.Status;
                enti.Adult = room.Adult;
                enti.Child = room.Child;
                return await db.SaveChangesAsync();
            }
            throw new Exception("Entity does not exist");
        }

        public async Task<bool> CheckRoom(string id, DateTime from, DateTime to, int invoice = 0)
        {
            var list = await db.Rooms.Include(r => r.Bookings)
                    .Where(b => b.Bookings.Any(b =>
                    ((b.CheckinDate.Date <= from.Date && from.Date <= b.CheckinDate.Date) ||
                    (b.CheckoutDate.Date <= to.Date && to.Date <= b.CheckoutDate.Date)) &&
                    (b.Status != "cancel") && (b.ID != invoice)))
                    .Select(r => r.ID)
                    .ToListAsync();
            if (list.Contains(id)) return false;
            return true;
        }
    }
}
