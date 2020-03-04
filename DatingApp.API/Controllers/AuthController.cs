using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.data.Interface;
using DatingApp.API.Dtos;
using DatingApp.API.model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthRepository _repo { get; }
        private IConfiguration _config { get; }

        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }
        //if you using apicontroller no need to set FROMBODY function
        [HttpPost("register")]
        public async Task<IActionResult> register(UserForRegister userForRegister)
        {
            // if(!ModelState.IsValid)
            //     return BadRequest(modelState);
            //that need for apicontroller not using in the up
            userForRegister.Username = userForRegister.Username.ToLower();
            var  bol = await _repo.UserExits(userForRegister.Username);
            if(!bol)
                return BadRequest("Username already exists");
            
            var userToCreate = new User
            {
                Username = userForRegister.Username
            };

            var CreateUser = await _repo.Register(userToCreate, userForRegister.Password);

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLogin userForLogin)
        {
            var userFromRepo = await _repo.Login(userForLogin.Username.ToLower(),userForLogin.Password);

            if(userFromRepo == null)
                return Unauthorized();
            
            var claims = new []
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSetting:Token").Value));

            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256Signature);

            var tokenDescripter = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescripter);

            return Ok(new {
                Token = tokenHandler.WriteToken(token)
            });
        }


    }
}