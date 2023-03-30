using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ResortProjectAPI.ModelEF;
using ResortProjectAPI.IServices;
using Microsoft.EntityFrameworkCore;

namespace ResortProjectAPI.Services
{
    public class RoomTypeService : IRoomTypeService
    {
        private readonly ResortDBContext db;
        public RoomTypeService(ResortDBContext context)
        {
            db = context;
        }
        public async Task<int> Create(RoomType type)
        {
            type.ID = type.ID.Trim().ToUpper();
            db.RoomTypes.Add(type);
            return await db.SaveChangesAsync();
        }

        public async Task<int> Edit(RoomType type)
        {
            RoomType enti = await db.RoomTypes.FindAsync(type.ID);
            if (enti != null)
            {
                enti.NameType = type.NameType;
                return db.SaveChanges();
            }
            throw new Exception("Entity does not exist");
        }

        public async Task<IEnumerable<RoomType>> GetAll() 
            => await db.RoomTypes.Include(r => r.Rooms).ToListAsync();

        public async Task<RoomType> GetByID(string id)
            => await db.RoomTypes.Include(r => r.Rooms).SingleOrDefaultAsync(r => r.ID == id);

        public async Task<int> Remove(string id)
        {
            RoomType type = await GetByID(id);
            db.RoomTypes.Remove(type);
            return await db.SaveChangesAsync();
        }
    }
}
