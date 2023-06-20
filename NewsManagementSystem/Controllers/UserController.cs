using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NewsManagementSystem.DataAccess.Repository.IRepository;
using NewsManagementSystem.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;using BCryptNet = BCrypt.Net.BCrypt;

namespace NewsManagementSystem.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class UserController : Controller
    {
        public IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IConfiguration config, IUnitOfWork unitOfWork)
        {
            _configuration = config;
            _unitOfWork = unitOfWork;
        }
        [HttpPost]
        public async Task<IActionResult> Post(User _userData)
        {
            if (_userData != null && _userData.UserName != null && _userData.Password != null)
            {
                var user =  GetUser(_userData.UserName, _userData.Password);

                if (user.Result != null)
                {
                    var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("Id", user.Result.Id.ToString()),
                        new Claim("UserName", user.Result.UserName)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(10),
                        signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        private async Task<User> GetUser(string userName, string password)
        {
           var user =  _unitOfWork.User.GetFirstOrDefault(u => u.UserName == userName);
            if (BCryptNet.Verify(password, user.Password))
                return user;
            else return null;
        }
    }
}
