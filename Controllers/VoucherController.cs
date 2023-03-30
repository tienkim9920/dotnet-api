using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ResortProjectAPI.IServices;
using ResortProjectAPI.ModelEF;
using Microsoft.AspNetCore.Authorization;

namespace ResortProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IVoucherService service;
        public VoucherController(IVoucherService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<Voucher>> GetAll() => await service.GetAll();

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(string id)
        {
            var result = await service.GetByID(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost("create")]
        [Authorize(Roles = "MANAGER")]
        public async Task<IActionResult> Create(Voucher model)
        {
            model.FromDate = model.FromDate.AddHours(7);
            model.ToDate = model.ToDate.AddHours(7);
            if (!ModelState.IsValid) return BadRequest(ModelState.Values);
            if (model.FromDate <= DateTime.Today.Date) return BadRequest("Begin day of voucher can not smaller than today");
            if (model.FromDate > model.ToDate) return BadRequest("Time of voucher not valid");
            if (await service.GetByID(model.Code) != null) return BadRequest("Voucher code was existed");
            try
            {
                await service.Create(model);
                return Ok();
            }
            catch
            {
                return BadRequest("Server can not create");
            }
        }

        [HttpPost("edit")]
        [Authorize(Roles = "MANAGER")]
        public async Task<IActionResult> Edit(Voucher model)
        {
            model.FromDate = model.FromDate.AddHours(7);
            model.ToDate = model.ToDate.AddHours(7);
            if (!ModelState.IsValid) return BadRequest(ModelState.Values);
            if (model.FromDate > model.ToDate) return BadRequest("Time of voucher not valid");
            try
            {
                await service.Edit(model);
                return Ok();
            }
            catch
            {
                return BadRequest("Server can not edit voucher");
            }

        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "MANAGER")]
        public async Task<IActionResult> Remove(string id)
        {
            if (await service.GetByID(id) == null) return NotFound();
            try
            {
                await service.Remove(id);
                return Ok();
            }
            catch
            {
                return BadRequest("Server can not remove");
            }
        }
    }
}
