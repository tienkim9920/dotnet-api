using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResortProjectAPI.IServices;
using ResortProjectAPI.ModelEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResortProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService service;

        public ServiceController(IServiceService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<Service>> GetAll() => await service.GetAll();

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(string id)
        {
            var result = await service.GetByID(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost("create")]
        [Authorize(Roles = "MANAGER")]
        public async Task<IActionResult> Create(Service model)
        {
            if (await service.GetByID(model.ID) != null) return BadRequest("ID was existed");
            if (!ModelState.IsValid) return BadRequest(ModelState.Values);
            try
            {
                await service.Create(model);
                return Ok();
            }
            catch
            {
                return BadRequest("Can not create service");
            }
        }

        [HttpPost("edit")]
        [Authorize(Roles = "MANAGER")]
        public async Task<IActionResult> Edit(Service model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values);
            if (await service.GetByID(model.ID) == null) return NotFound();
            try
            {
                await service.Edit(model);
                return Ok();
            }
            catch
            {
                return BadRequest("Server can not edit service");
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
