using Microsoft.AspNetCore.Mvc;
using System;
using ResortProjectAPI.ModelRequest;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using ResortProjectAPI.IServices;

namespace ResortProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ICustomerService _customer;
        private readonly IStaffService _staff;
        private readonly IConfiguration _config;

        public AuthController(ICustomerService customer,IStaffService staff, IConfiguration config)
        {
            _customer = customer;
            _staff = staff;
            _config = config;
        }

        [HttpPost,Route("user")]
        public async Task<IActionResult> UserLogin(LoginModel model)
        {
            if (!ModelState.IsValid) return BadRequest("Model is invalid" + ModelState.Values);
            var user = await _customer.GetByID(model.Username);
            if (user == null) return NotFound("User not exist");
            return user.Password != model.Password ? BadRequest("Password was wrong") : Ok(new { token = GenerateJSONWebToken(model, "CLIENT", user.Name) });
        }

        [HttpPost, Route("staff")]
        public async Task<IActionResult> StaffLogin( LoginModel model)
        {
            if (!ModelState.IsValid) return BadRequest("Model is invalid: " + ModelState.Values);
            var staff = await _staff.GetById(model.Username);
            if (staff == null) return NotFound("Staff not exist");
            return staff.Password != model.Password ? BadRequest("Password was wrong") : Ok(new { token = GenerateJSONWebToken(model, staff.PermissionID, staff.Name) });
        }

        //Generate JWT
        [NonAction]
        private string GenerateJSONWebToken(LoginModel model, string role, string name)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:JWT_Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, model.Username),
                //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role,role.ToUpper()),
                new Claim("name",name),
                new Claim("role",role.ToUpper())
            };
            var token = new JwtSecurityToken(
                issuer: _config["JWT:Issuer"],
                audience: _config["JWT:Issuer"],
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
