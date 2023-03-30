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
    [Authorize(Roles = "WAREHOUSE, MANAGER")]
    public class DistributionSPController : ControllerBase
    {
        private readonly ISuppliesForRoomService service;
        private readonly IRoomService room;
        private readonly ISupplyService sp;

        public DistributionSPController(ISuppliesForRoomService service, IRoomService room, ISupplyService sp)
        {
            this.service = service;
            this.room = room;
            this.sp = sp;
        }

        [HttpGet("room/{id}")]
        public async Task<IActionResult> GetSpOfRoom(string id)
        {
            if (await room.GetByID(id) == null) return NotFound();
            return Ok(await service.GetSuppliesOfRoom(id));
        }

        [HttpGet("supply/{id}")]
        public async Task<IActionResult> GetRoomOfSp(string id)
        {
            if (await sp.GetByID(id) == null) return NotFound();
            return Ok(await service.GetRoomsOfSupply(id));
        }

        [HttpPost("givesp4room")]
        public async Task<IActionResult> GiveSpForRoom(SuppliesForRoom model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values);
            Supply sup = await sp.GetByID(model.SupplyID);
            if (sup == null) return BadRequest("Vật tư không tồn tại");
            if (sup.Total < model.Count) return BadRequest("Số lượng không khả dụng");
            if (await room.GetByID(model.RoomID) == null) return BadRequest("Phòng không tồn tại");
            try
            {
                await service.GiveSPForRoom(model);
                return Ok();
            }
            catch
            {
                return BadRequest("Can not distribution");
            }
        }

        [HttpPost("removespFromRoom")]
        public async Task<IActionResult> RemoveSpFromRoom(SuppliesForRoom model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values);
            if (await room.GetByID(model.RoomID) == null) return BadRequest("Phòng không tồn tại");
            if (await sp.GetByID(model.SupplyID) == null) return BadRequest("Vật tư không tồn tại");
            try
            {
                await service.RemoveSPFromRoom(model);
                return Ok();
            }
            catch
            {
                return BadRequest("Can not remove supply from room");
            }
        }
    }
}
