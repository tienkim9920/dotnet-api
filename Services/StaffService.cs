using Microsoft.EntityFrameworkCore;
using ResortProjectAPI.IServices;
using ResortProjectAPI.ModelEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ResortProjectAPI.ModelRequest;

namespace ResortProjectAPI.Services
{
    public class StaffService : IStaffService
    {
        private readonly ResortDBContext _context;

        public StaffService(ResortDBContext context)
        {
            _context = context;
        }

        public async Task<int> Create(StaffRequest model)
        {
            try
            {
                if (model.PermissionID==null)
                {
                    model.PermissionID = "STAFF";
                }
                Permission permission = _context.Permissions.Find(model.PermissionID);
                if(model.Birth == null)
                {
                    model.Birth = DateTime.Parse("1990-01-01");
                }
                Staff staff = new()
                {
                    ID = model.ID,
                    Name = model.Name,
                    Gender = model.Gender,
                    Password = model.Password,
                    Phone = model.Phone,
                    Email = model.Email,
                    Birth = (DateTime)model.Birth,
                    PermissionID = model.PermissionID,
                    Permission = permission
                };
                _context.Staffs.Add(staff);
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Create was fail", ex);
            }
        }

        public async Task<int> Delete(string staffID)
        {
            var staff = await _context.Staffs.FindAsync(staffID);
            _context.Staffs.Remove(staff);
            return await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Staff>> GetAll()
        {
            return await _context.Staffs.Include(s => s.Permission).ToListAsync();
        }

        public async Task<Staff> GetById(string staffID)
        {
            var staff = await _context.Staffs.FindAsync(staffID);
            return staff;
        }

        public async Task<int> Update(Staff model)
        {
            var staff = await _context.Staffs.FindAsync(model.ID);
            staff.Name = model.Name;
            staff.Gender = model.Gender;
            staff.Birth = model.Birth;
            staff.Email = model.Email;
            staff.Phone = model.Phone;
            staff.Password = model.Password;
            staff.PermissionID = model.PermissionID;
            return await _context.SaveChangesAsync();
        }
    }
}
