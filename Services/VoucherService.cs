using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ResortProjectAPI.IServices;
using ResortProjectAPI.ModelEF;

namespace ResortProjectAPI.Services
{
    public class VoucherService : IVoucherService
    {
        private readonly ResortDBContext db;
        public VoucherService(ResortDBContext context)
        {
            db = context;
        }
        public async Task<int> Create(Voucher voucher)
        {
            voucher.Code = voucher.Code.Trim().ToUpper();
            db.Vouchers.Add(voucher);
            return await db.SaveChangesAsync();
        }

        public async Task<int> Edit(Voucher voucher)
        {
            Voucher enti = await db.Vouchers.SingleOrDefaultAsync(s => s.Code.ToLower() == voucher.Code.Trim().ToLower());
            if (enti != null)
            {
                enti.Condition = voucher.Condition;
                enti.Discount = voucher.Discount;
                enti.FromDate = voucher.FromDate.Date;
                enti.ToDate = voucher.ToDate.Date;
                return await db.SaveChangesAsync();
            }
            throw new Exception("Entity does not exist");
        }

        public async Task<IEnumerable<Voucher>> GetAll()
        => await db.Vouchers.ToListAsync();

        public async Task<Voucher> GetByID(string id)
        => await db.Vouchers.FindAsync(id);

        public async Task<int> Remove(string id)
        {
            Voucher voucher = await db.Vouchers.FindAsync(id);
            db.Vouchers.Remove(voucher);
            return await db.SaveChangesAsync();
        }
    }
}
