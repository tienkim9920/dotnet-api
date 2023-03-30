using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ResortProjectAPI.ModelEF;
using ResortProjectAPI.IServices;
using Microsoft.EntityFrameworkCore;

namespace ResortProjectAPI.Services
{
    public class ImageService:IImageService
    {
        private readonly ResortDBContext db;
        public ImageService(ResortDBContext context)
        {
            db = context;
        }

        public async Task<int> Create(Image img)
        {
            db.Images.Add(img);
            return await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Image>> GetByIDEnti(string id)
            => await db.Images.Where(i => i.RoomID == id).ToListAsync();

        public async Task<Image> GetByURL(string url)
        {
            var result = await db.Images.Where(i => i.URL == url).SingleOrDefaultAsync();
            return result;
        }

        public async Task<int> Remove(string url)
        {
            Image img = await db.Images.FindAsync(url);
            db.Images.Remove(img);
            return await db.SaveChangesAsync();
        }
    }
}
