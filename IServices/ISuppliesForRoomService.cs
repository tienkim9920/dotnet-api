using ResortProjectAPI.ModelEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResortProjectAPI.IServices
{
    public interface ISuppliesForRoomService
    {
        Task<IEnumerable<SuppliesForRoom>> GetSuppliesOfRoom(string roomID);
        Task<IEnumerable<SuppliesForRoom>> GetRoomsOfSupply(string supID);
        Task<SuppliesForRoom> Single(string roomID, string supID);
        Task<int> Create(SuppliesForRoom model);
        Task<int> GiveSPForRoom(SuppliesForRoom model);
        Task<int> Remove(string roomID, string supID);
        Task<int> RemoveSPFromRoom(SuppliesForRoom model);
    }
}
