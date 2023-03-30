using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ResortProjectAPI.ModelEF;
using ResortProjectAPI.IServices;
using Microsoft.EntityFrameworkCore;

namespace ResortProjectAPI.Services
{
    public class ServiceService:IServiceService
    {
        private readonly ResortDBContext db;
        public ServiceService(ResortDBContext context)
        {
            db = context;
        }

        public async Task<int> Create(Service service)
        {
            service.ID = service.ID.Trim().ToUpper();
            service.Description = service.Description.Trim();
            db.Services.Add(service);
            return await db.SaveChangesAsync();
        }

        public async Task<int> Edit(Service service)
        {
            Service enti = await db.Services.SingleOrDefaultAsync(s => s.ID == service.ID);
            if (enti != null)
            {
                enti.Name = service.Name;
                enti.Description = service.Description;
                enti.Price = service.Price;
                return await db.SaveChangesAsync();
            }
            throw new Exception("Entity does not exist");
        }

        public async Task<IEnumerable<Service>> GetAll()
            => await db.Services.ToListAsync();

        public async Task<Service> GetByID(string id)
            => await db.Services.FindAsync(id);

        public async Task<int> Remove(string id)
        {
            Service service = await db.Services.FindAsync(id);
            db.Services.Remove(service);
            return await db.SaveChangesAsync();
        }
    }
}
