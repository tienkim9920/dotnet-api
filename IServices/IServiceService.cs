using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ResortProjectAPI.ModelEF;

namespace ResortProjectAPI.IServices
{
    public interface IServiceService
    {
        Task<IEnumerable<Service>> GetAll();
        Task<Service> GetByID(string id);
        Task<int> Create(Service service);
        Task<int> Remove(string id);
        Task<int> Edit(Service service);
    }
}
