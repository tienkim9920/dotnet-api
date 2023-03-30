using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ResortProjectAPI.ModelEF;

namespace ResortProjectAPI.IServices
{
    public interface IImageService
    {
        Task<IEnumerable<Image>> GetByIDEnti(string id);
        Task<Image> GetByURL(string url);
        Task<int> Create(Image img);
        Task<int> Remove(string url);
    }
}
