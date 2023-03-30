using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ResortProjectAPI.IServices;
using ResortProjectAPI.ModelEF;
using ResortProjectAPI.ModelRequest;

namespace ResortProjectAPI.Services
{
    public class SupplyService : ISupplyService
    {
        private readonly ResortDBContext db;

        public SupplyService(ResortDBContext context)
        {
            db = context;
        }
        public async Task<int> Create(Supply sup)
        {
            sup.ID = sup.ID.Trim().ToUpper();
            db.Supplies.Add(sup);
            return await db.SaveChangesAsync();
        }

        public async Task<int> Delete(string id)
        {
            Supply sup = await db.Supplies.FindAsync(id);
            db.Supplies.Remove(sup);
            return db.SaveChanges();
        }

        public async Task<IEnumerable<Supply>> GetAll()
        {
            return await db.Supplies.ToListAsync();
        }

        public async Task<Supply> GetByID(string id)
        {
            return await db.Supplies
                .Include(s => s.Rooms)
                .ThenInclude(sr => sr.Room)
                .SingleOrDefaultAsync(s => s.ID == id);
        }

        public async Task<int> Update(SupplyModelRequest model)
        {
            Supply enti =
                    await db.Supplies
                    .Include(s => s.Rooms)
                    .SingleOrDefaultAsync(s => s.ID == model.id);
            if (enti != null)
            {
                switch (model.editType)
                {
                    case "newcount":
                        enti.Total = (int)model.count;
                        foreach (var item in db.SuppliesForRooms) db.SuppliesForRooms.Remove(item);
                        break;
                    case "addcount":
                        enti.Total += (int)model.count;
                        break;
                    default: break;
                }
                enti.Name = model.name;
                return await db.SaveChangesAsync();
            }
            throw new Exception("Entity does not exist");
        }
    }
}
