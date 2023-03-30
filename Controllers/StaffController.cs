using Microsoft.AspNetCore.Mvc;
using ResortProjectAPI.IServices;
using ResortProjectAPI.ModelEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ResortProjectAPI.ModelRequest;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ResortProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ADMIN, MANAGER")]
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _service;
        public StaffController(IStaffService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(string id)
        {
            var entity = await _service.GetById(id);
            if (entity == null) return NotFound($"Staff {id} does not exist");
            return Ok(new { staff = entity });
        }

        [HttpGet]
        public async Task<IEnumerable<Staff>> GetAll()
        {
            return await _service.GetAll();
        }

        [HttpPost("add")]
        public async Task<IActionResult> Create(StaffRequest staff)
        {
            staff.Birth = staff.Birth.AddHours(7);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }
            if (await _service.GetById(staff.ID) != null) return BadRequest("User exist");
            try
            {
                if (await _service.Create(staff) > 0) return Ok($"Staff {staff.ID} was created");
                return BadRequest("Server can not create staff");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update(Staff model)
        {
            model.Birth = model.Birth.AddHours(7);
            if (!ModelState.IsValid) return BadRequest(ModelState.Values);
            try
            {
                var staff = await _service.GetById(model.ID);
                if (staff == null) return NotFound($"Staff {model.ID} does not exist");
                await _service.Update(model);
                return Ok();
            }
            catch
            {
                return BadRequest("Server can not update staff");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var staff = await _service.GetById(id);
                if (staff == null) return NotFound($"Staff {id} does not exist");
                if (await _service.Delete(id) > 0) return Ok($"Staff {id} was remove");
                return BadRequest($"Can not remove staff {id}");
            }
            catch
            {
                return BadRequest("Server can not remove staff");
            }
        }
    }
}
