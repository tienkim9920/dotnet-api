using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ResortProjectAPI.ModelEF;

namespace ResortProjectAPI.IServices
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAll();

        Task<Customer> GetByID(string id);

        Task<int> Create(Customer customer);

        Task<int> Update(Customer customer);

        Task<int> Delete(string id);

        Task<bool> CanBooking(string id);
    }
}
