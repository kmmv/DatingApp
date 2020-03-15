using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace DatingApp.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration  _config;

        public AuthController(IAuthRepository repo, IConfiguration  config)
        {
            _repo = repo;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

              if (await _repo.UserExists(userForRegisterDto.Username))  return BadRequest("Username already exists");

            var userToCreate = new User
            {
                Username = userForRegisterDto.Username
            };
 
            var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password);

            return StatusCode(201); 

        }

        [HttpPost("login")]

        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {

            // km: Check if the user exists
            var userFromRepo = await _repo.Login(userForLoginDto.Username.ToLower(),
                                                     userForLoginDto.Password);

            if(userFromRepo == null ) return Unauthorized();

            // km: Add some pieces of information to the token, variable is Claims
            // km: token will contain two claims - userid and username
            var  claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };

            // km : a security key is required to sign the token
            // km: AppSettings.Token value needs to be stored in the configuration
            // km: Key is stored as UTF8 bytes and the value is obtained from token

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            // signing credientials
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            //create token descriptpr, pass claims, 24 hours expiry date
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            // km:
            var tokenHandler = new JwtSecurityTokenHandler();

            // km: create token variable
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // km: write the token variable
            var result = new { token = tokenHandler.WriteToken(token)};

            return Ok(result);
           
        }

        
    }
}