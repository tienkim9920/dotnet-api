using ResortProjectAPI.ModelEF;
using System.Collections.Generic;
using System.Threading.Tasks;
using ResortProjectAPI.ModelRequest;

namespace ResortProjectAPI.IServices
{
    public interface IStaffService
    {
        Task<Staff> GetById(string staffID);

        Task<IEnumerable<Staff>> GetAll();

        Task<int> Create(StaffRequest staff);

        Task<int> Delete(string staffID);

        Task<int> Update(Staff staff);
    }
}
