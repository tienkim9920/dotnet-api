using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using ResortProjectAPI.IServices;
using ResortProjectAPI.ModelEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ResortProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService service;
        private readonly IImageService imgSV;

        public RoomController(IRoomService service, IImageService imgSV)
        {
            this.service = service;
            this.imgSV = imgSV;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await service.GetAll();
            return Ok(result);
        }

        [HttpGet, Route("{id}")]
        public async Task<IActionResult> Details(string id) 
        {
            var room = await service.GetByID(id);
            if (room == null) return NotFound();
            return Ok(room);
        }

        [HttpPost("create")]
        [Authorize(Roles = "MANAGER")]
        public async Task<IActionResult> Create(Room model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values);
            if (await service.GetByID(model.ID) != null) return BadRequest("Room was existed");
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

        [HttpPost("edit_info")]
        [Authorize(Roles = "MANAGER")]
        public async Task<IActionResult> EditInfo(Room model)
        {
            if (await service.GetByID(model.ID) == null) return NotFound();
            if (!ModelState.IsValid) return BadRequest(ModelState.Values);
            try
            {
                await service.Update(model);
                return Ok();
            }
            catch
            {
                return BadRequest("Server can not create");
            }
        }

        [HttpGet("img/{id}")]
        public async Task<IEnumerable<Image>> GetImage(string id) => await imgSV.GetByIDEnti(id);

        [HttpPost("add_img")]
        [Authorize(Roles = "MANAGER")]
        public async Task<IActionResult> AddImg(Image model)
        {
            //IEnumerable<string> listImg = data["listImg"].ToObject<IEnumerable<string>>();
            var room = await service.GetByID(model.RoomID);
            if (room == null) return NotFound();
            try
            {
                await imgSV.Create(model);
                return Ok();
            }
            catch
            {
                return BadRequest("Server can not add image");
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
                return BadRequest("Server can not remove room");
            }
        }

        [HttpPost("remove_img")]
        public async Task<IActionResult> RemoveImg(string url)
        {
            var img = await imgSV.GetByURL(url);
            if (img == null) return NotFound();
            try
            {
                await imgSV.Remove(url);
                return Ok();
            }
            catch
            {
                return BadRequest("Server can not remove image");
            }
        }

        [HttpGet("img-get")]
        public async Task<Image> select(string url) => await imgSV.GetByURL(url);
    }
}
