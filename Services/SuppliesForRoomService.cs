using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ResortProjectAPI.IServices;
using ResortProjectAPI.ModelEF;

namespace ResortProjectAPI.Services
{
    public class SuppliesForRoomService : ISuppliesForRoomService
    {
        private readonly ResortDBContext db;

        public SuppliesForRoomService(ResortDBContext context)
        {
            db = context;
        }
        public async Task<int> Create(SuppliesForRoom model)
        {
            var sup = await db.Supplies.FindAsync(model.SupplyID);
            sup.Total -= model.Count;
            db.SuppliesForRooms.Add(model);
            return await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<SuppliesForRoom>> GetRoomsOfSupply(string supID)
        {
            return
                    await db.SuppliesForRooms
                    .Include(s => s.Room)
                    .Where(s => s.SupplyID == supID)
                    .ToListAsync();
        }

        public async Task<IEnumerable<SuppliesForRoom>> GetSuppliesOfRoom(string roomID)
        {
            return
                    await db.SuppliesForRooms
                    .Include(s => s.Supply)
                    .Where(s => s.RoomID == roomID)
                    .ToListAsync();
        }

        public async Task<int> GiveSPForRoom(SuppliesForRoom model)
        {
            SuppliesForRoom enti = await db.SuppliesForRooms.Where(sr => sr.RoomID == model.RoomID && sr.SupplyID == model.SupplyID).SingleOrDefaultAsync();
            if (enti == null) return await Create(model);
            else
            {
                var sup = await db.Supplies.FindAsync(model.SupplyID);
                sup.Total -= model.Count;
                enti.Count += model.Count;
            }
            return await db.SaveChangesAsync();
        }

        public async Task<int> Remove(string roomID, string supID)
        {
            var enti =
                   await db.SuppliesForRooms
                   .Include(s => s.Supply)
                   .FirstOrDefaultAsync(sr => sr.RoomID == roomID && sr.SupplyID == supID);
            Supply sp = await db.Supplies.FindAsync(supID);
            sp.Total += enti.Count;
            db.SuppliesForRooms.Remove(enti);
            return await db.SaveChangesAsync();
        }

        public async Task<int> RemoveSPFromRoom(SuppliesForRoom model)
        {
            var enti = await db.SuppliesForRooms.Where(sr => sr.RoomID == model.RoomID && sr.SupplyID == model.SupplyID).SingleOrDefaultAsync();
            if (enti.Count <= model.Count) return await Remove(model.RoomID, model.SupplyID);
            Supply sp = await db.Supplies.FindAsync(model.SupplyID);
            sp.Total += model.Count;
            enti.Count -= model.Count;
            return await db.SaveChangesAsync();
        }

        public async Task<SuppliesForRoom> Single(string roomID, string supID)
        {
            return await db.SuppliesForRooms.Where(sr => sr.RoomID == roomID && sr.SupplyID == supID).SingleOrDefaultAsync();
        }
    }
}
