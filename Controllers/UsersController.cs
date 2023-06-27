using API.Data;
using Microsoft.AspNetCore.Mvc;
using API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using API.Interfaces;
using AutoMapper;
using API.DTOs;

namespace API.Controllers
{

    [Authorize] // this means that the user must be logged in to access this controller
    public class UsersController : BaseApiController
    {

        private readonly IUserRepository _userRepository;
        public IMapper _mapper { get; }

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }



        [HttpGet] // essentially we are defining the route here - think of flask routes (app.route('/users')
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await _userRepository.GetMembersAsync();


            return Ok(users); // this will return the users to the client- Ok is a method that returns a 200 response without it we would have to return an ActionResult



        }


        [HttpGet("{username}")] // we are defining the route here for one user - think of flask routes (app.route('/users/<username>')

        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            return await _userRepository.GetMemberAsync(username); // this will get a user by their username from the database


        }
    }
}