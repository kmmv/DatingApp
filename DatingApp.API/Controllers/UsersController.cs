using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace DatingApp.API.Controllers
{

    // The service filter above will invoke the LogUserActivity service referenced on the startup class
    // The logUserActivity will capture Last Active datetime whenever the user interacts with any of the methods below
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        public UsersController(IDatingRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;

        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        {

            // we receive pagelist of users
            var users = await _repo.GetUsers(userParams);

            var usersToReurn = _mapper.Map<IEnumerable<UserForListDto>>(users);

            // because we are inside API controller we have access to the response
            // we have extention method AddPgination on the response
            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
        
            return Ok(usersToReurn);
        }

        [HttpGet("{id}", Name="GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repo.GetUser(id);

            var userToReurn = _mapper.Map<UserForDetailedDto>(user);

            return Ok(userToReurn);

        }

        // the following method is used to persist changes on the member update user
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _repo.GetUser(id);

            _mapper.Map(userForUpdateDto, userFromRepo);

            if (await _repo.SaveAll())
                return NoContent();

            throw new Exception($"Updating user {id} failed on save");
        }

    }
}