using ResortProjectAPI.ModelEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResortProjectAPI.IServices
{
    public interface IVoucherService
    {
        Task<IEnumerable<Voucher>> GetAll();
        Task<Voucher> GetByID(string id);
        Task<int> Create(Voucher voucher);
        Task<int> Remove(string id);
        Task<int> Edit(Voucher voucher);
    }
}
