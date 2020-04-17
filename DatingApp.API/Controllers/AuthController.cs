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
using AutoMapper;

namespace DatingApp.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public AuthController(IAuthRepository repo,
                              IConfiguration config,
                              IMapper mapper)
        {
            _repo = repo;
            _config = config;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {

            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            if (await _repo.UserExists(userForRegisterDto.Username)) return BadRequest("Username already exists");

            /*var userToCreate = new User
            {
                Username = userForRegisterDto.Username
            };*/

            // We map from the Source (in the case of Registering the UserForRegisterDto) on the left to the User (destination) on the right.
            var userToCreate = _mapper.Map<User>(userForRegisterDto);

            // use of userForRegisterDto
            //We don't want to rely on client validation alone as this can be easily bypassed.  
            // We always want to validate the data coming into our server even if it does mean duplicating the effort here.
            var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password);

            // we want to send only selected information (excluding password)
            var userToReturn = _mapper.Map<UserForDetailedDto>(createdUser);

            //reates a Microsoft.AspNetCore.Mvc.CreatedAtRouteResult object that 
            //produces a Microsoft.AspNetCore.Http.StatusCodes.Status201Created response.
            // CreatedAtRoute params: controller method, controller method value and the details to return
            return CreatedAtRoute("GetUser", new {controller = "Users", Id= createdUser.Id}, userToReturn);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            // km: Check if the user exists
            var userFromRepo = await _repo.Login(userForLoginDto.Username.ToLower(),
                                                     userForLoginDto.Password);

            if (userFromRepo == null) return Unauthorized();

            // km: Add some pieces of information to the token, variable is Claims
            // km: token will contain two claims - userid and username
            var claims = new[]
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

            // map a userListDto for the purpose of showing the user photo on the navbar near Welcome message
            var user = _mapper.Map<UserForListDto>(userFromRepo);

            return Ok(new
                {
                // km: write the token variable
                token = tokenHandler.WriteToken(token),
                // km: return the user along the token for displaying photo on the navbar near Welcome message
                user
                }          
            );

        }


    }
}