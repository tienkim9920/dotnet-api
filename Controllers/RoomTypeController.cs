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
    public class RoomTypeController : ControllerBase
    {
        private readonly IRoomTypeService service;

        public RoomTypeController(IRoomTypeService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<RoomType>> GetAll() => await service.GetAll();

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(string id)
        {
            var result = await service.GetByID(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost("create")]
        [Authorize(Roles = "MANAGER")]
        public async Task<IActionResult> Create(RoomType model)
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
        public async Task<IActionResult> Edit(RoomType model)
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
                return BadRequest("Server can not edit");
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
