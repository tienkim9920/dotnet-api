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
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _service;
        public CustomerController(ICustomerService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<Customer>> GetAll() => await _service.GetAll();

        [HttpGet,Route("{id}")]
        public async Task<IActionResult> GetByID(string id)
        {
            var customer = await _service.GetByID(id);
            if (customer is null) return NotFound();
            return Ok(customer);
        }

        [HttpPost("create")]
        [Authorize(Roles = "ADMIN, MANAGER")]
        public async Task<IActionResult> Create( Customer model)
        {
            model.Birth = model.Birth.AddHours(7);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }
            var customer = await _service.GetByID(model.ID);
            if (customer != null) return BadRequest("Customer ID existed");
            if (await _service.Create(model) > 0) return Ok(model.ID);
            return BadRequest("Server can not create customer");
        }

        [HttpPost,Route("update")]
        [Authorize(Roles = "ADMIN, MANAGER")]
        public async Task<IActionResult> Update(Customer model)
        {
            model.Birth = model.Birth.AddHours(7);
            if (!ModelState.IsValid) return BadRequest(ModelState.Values);
            var customer = await _service.GetByID(model.ID);
            if (customer == null) return BadRequest("Customer does not exist");
            if (await _service.Update(model) > 0) return Ok($"Customer {model.ID} has been update");
            return BadRequest("Server can not update customer");
        }

        [HttpDelete, Route("{id}")]
        [Authorize(Roles = "ADMIN, MANAGER")]
        public async Task<IActionResult> Delete(string id)
        {
            var customer = await _service.GetByID(id);
            if (customer == null) return BadRequest("Customer does not exist");
            try
            {
                if (await _service.Delete(id) > 0) return Ok($"Customer {id} has been remove");
                return BadRequest($"Can not remove customer {id}");
            }
            catch
            {
                return BadRequest("Server can not remove customer");
            }
        }
    }
}
