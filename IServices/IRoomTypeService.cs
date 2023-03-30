using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ResortProjectAPI.ModelEF;

namespace ResortProjectAPI.IServices
{
    public interface IRoomTypeService
    {
        Task<IEnumerable<RoomType>> GetAll();
        Task<RoomType> GetByID(string id);
        Task<int> Create(RoomType type);
        Task<int> Remove(string id);
        Task<int> Edit(RoomType type);
    }
}
