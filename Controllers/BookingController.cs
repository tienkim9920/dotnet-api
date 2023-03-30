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
    [Authorize(Roles = "STAFF, MANAGER")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _service;
        private readonly IServiceService _serviceService;
        private readonly ICustomerService _customer;
        private readonly IRoomService _room;
        private readonly IVoucherService _voucher;
        public BookingController(IBookingService service, ICustomerService customer, IRoomService room, IVoucherService voucher, IServiceService serviceService)
        {
            _service = service;
            _customer = customer;
            _room = room;
            _voucher = voucher;
            _serviceService = serviceService;
        }

        [HttpGet]
        public async Task<IEnumerable<Booking>> GetAll() => await _service.GetAll();

        [HttpGet,Route("{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var result = await _service.GetByID(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(Booking model)
        {
            if (model.VoucherCode == "") model.VoucherCode = null;
            model.CheckinDate = model.CheckinDate.AddHours(7);
            model.CheckoutDate = model.CheckoutDate.AddHours(7);
            if (!ModelState.IsValid) return BadRequest(ModelState.Values);
            if (!(await _customer.CanBooking(model.CustomerID))) return BadRequest($"Customer {model.CustomerID} already have an order and can not book more");
            if(model.CheckinDate.Date >= model.CheckoutDate.Date)  return BadRequest("Check in date must greater than check out date");
            if (!(await _room.CheckRoom(model.RoomID, model.CheckinDate, model.CheckoutDate))) return BadRequest($"Can not book this room from {model.CheckinDate.ToString("dd/MM/yyyy")} to {model.CheckoutDate.ToString("dd/MM/yyyy")}");
            var room = await _room.GetByID(model.RoomID);
            if (room.Adult < model.Adult) return BadRequest($"Max adult of room {model.RoomID} is {room.Adult}");
            if (room.Child < model.Child) return BadRequest($"Max child of room {model.RoomID} is {room.Child}");
            
            if(model.VoucherCode != "")
            {
                var voucher = await _voucher.GetByID(model.VoucherCode);
                if (voucher == null) return NotFound($"Voucher {model.VoucherCode} not found");
                if (!(voucher.FromDate.Date <= model.CheckinDate.Date && model.CheckinDate.Date <= voucher.ToDate.Date) ||
                !(voucher.FromDate.Date <= model.CheckoutDate.Date && model.CheckoutDate.Date <= voucher.ToDate.Date))
                    return BadRequest($"Voucher {model.VoucherCode} can apply for bill booking from {voucher.FromDate.ToString("dd/MM/yyyy")} to {voucher.ToDate.ToString("dd/MM/yyyy")}");
                double tmp = room.Price * (int)model.CheckoutDate.Date.Subtract(model.CheckinDate.Date).Days;
                if (tmp < voucher.Condition)
                    return BadRequest($"Voucher {model.VoucherCode} can apply for bill with min value is {voucher.Condition}");
            }
            else
            {
                model.VoucherCode = null;
            }
            
            try
            {
                await _service.Create(model);
                return Ok();
            }
            catch(Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return BadRequest("Can not create new order cause some problems");
            }
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit(Booking model)
        {
            if (model.VoucherCode == "") model.VoucherCode = null;
            model.CheckinDate = model.CheckinDate.AddHours(7);
            model.CheckoutDate = model.CheckoutDate.AddHours(7);
            if (!ModelState.IsValid) return BadRequest(ModelState.Values);
            if (model.CheckinDate.Date >= model.CheckoutDate.Date) return BadRequest("Check in date must greater than check out date");
            if (!(await _room.CheckRoom(model.RoomID, model.CheckinDate, model.CheckoutDate, model.ID))) return BadRequest($"Can not book this room from {model.CheckinDate.ToString("dd/MM/yyyy")} to {model.CheckoutDate.ToString("dd/MM/yyyy")}");
            var room = await _room.GetByID(model.RoomID);
            if (room.Adult < model.Adult) return BadRequest($"Max adult of room {model.RoomID} is {room.Adult}");
            if (room.Child < model.Child) return BadRequest($"Max child of room {model.RoomID} is {room.Child}");
            if (model.VoucherCode != "")
            {
                var voucher = await _voucher.GetByID(model.VoucherCode);
                if (voucher == null) return BadRequest("Can not find voucher " + model.VoucherCode);
                if (!(voucher.FromDate.Date <= model.CheckinDate.Date && model.CheckinDate.Date <= voucher.ToDate.Date) ||
                !(voucher.FromDate.Date <= model.CheckoutDate.Date && model.CheckoutDate.Date <= voucher.ToDate.Date))
                    return BadRequest($"Voucher {model.VoucherCode} can apply for bill booking from {voucher.FromDate.ToString("dd/MM/yyyy")} to {voucher.ToDate.ToString("dd/MM/yyyy")}");
                model.Services = (await _service.GetByID(model.ID)).Services;
                var tmp = await GetPrice(model);
                if (tmp < voucher.Condition)
                    return BadRequest($"Voucher {model.VoucherCode} can apply for bill with min value is {voucher.Condition}");
            }
            else
            {
                model.VoucherCode = null;
            }
            try
            {
                await _service.Edit(model);
                return Ok();
            }
            catch
            {
                return BadRequest("Can not create new order cause some problems");
            }
        }

        [HttpPost,Route("check/{id}")]
        public async Task<IActionResult> AcceptBill(int id, string status)
        {
            try
            {
                await _service.CheckBooking(id, status);
                return Ok();
            }
            catch
            {
                return BadRequest("Error when trying to accept bill");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            try
            {
                await _service.Remove(id);
                return Ok();
            }
            catch
            {
                return BadRequest("Error when trying to remove bill");
            }
        }

        [HttpPost("add-sv")]
        public async Task<IActionResult> AddSV(BookingServices model)
        {
            try
            {
                await _service.AddSV(model);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("remove-sv")]
        public async Task<IActionResult> RemoveSV(BookingServices model)
        {
            try
            {
                await _service.RemoveSV(model);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("customer")]
        public async Task<IEnumerable<Customer>> CustomerCanBook() => await _service.GetCustomerAvailable();

        [HttpGet("service/{id}")]
        public async Task<IEnumerable<string>> ServiceOfBill(int id) => await _service.GetServicesOfBill(id);

        [NonAction]
        private async Task<double> GetPrice(Booking booking)
        {
            var room = await _room.GetByID(booking.RoomID);

            if(room == null)
            {
                return 0;
            }

            double price = room.Price * booking.CheckoutDate.Date.Subtract(booking.CheckinDate.Date).Days;

            var temp = await _service.GetByID(booking.ID);
            /*var services;
            if(await _service.GetByID(booking.ID) != null)
            {
                var services = await _serviceService.GetAll();
            } else
            {
                
            }//*/
            //List<Service> serviceList = services.Where(item => item.)
            if (booking.Services.Count() > 0)
            {
                foreach (var sv in booking.Services)
                {
                    price += sv.Service.Price;
                }
            }
            if (booking.VoucherCode != null)
            {
                price = price * ((100 - booking.Voucher.Discount) / 100.0);
            }
            return price;
        }
    }
}
