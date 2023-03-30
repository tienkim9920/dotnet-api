using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ResortProjectAPI.IServices;
using ResortProjectAPI.ModelEF;
using ResortProjectAPI.ModelRequest;
using Microsoft.AspNetCore.Authorization;

namespace ResortProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplyController : ControllerBase
    {
        private readonly ISupplyService service;

        public SupplyController(ISupplyService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<Supply>> GetAll() => await service.GetAll();

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(string id)
        {
            var result = await service.GetByID(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost("create")]
        [Authorize(Roles = "MANAGER")]
        public async Task<IActionResult> Create(Supply model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values);
            if (await service.GetByID(model.ID) != null) return BadRequest("ID was existed");
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
        public async Task<IActionResult> Edit(SupplyModelRequest model)
        {
            if(model.count < 0 && model.editType != "none")
            {
                return BadRequest("Enter valid number for edit");
            }
            if (!ModelState.IsValid) return BadRequest(ModelState.Values);
            var result = await service.GetByID(model.id);
            if (result == null) return NotFound();
            try
            {
                await service.Update(model);
                return Ok();
            }
            catch
            {
                return BadRequest("Server can not update");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "MANAGER")]
        public async Task<IActionResult> Remove(string id)
        {
            if (await service.GetByID(id) == null) return NotFound();
            try
            {
                await service.Delete(id);
                return Ok();
            }
            catch
            {
                return BadRequest("Server can not remove");
            }
        }
    }
}
