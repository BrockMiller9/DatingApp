

using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using API.DTOs;
using Microsoft.EntityFrameworkCore;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly ITokenService _tokenService;
        public IMapper _mapper { get; }
        private readonly UserManager<AppUser> _userManager;
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        [HttpPost("register")] // POST: api/account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)//takes in the username and password and returns the app user
        {
            if (await UserExists(registerDto.Username)) return BadRequest("Username is taken"); //checks if the username exists in the database

            var user = _mapper.Map<AppUser>(registerDto); //maps the register dto to the app user

            user.UserName = registerDto.Username.ToLower(); //sets the username to lowercase

            var result = await _userManager.CreateAsync(user, registerDto.Password); //creates the user

            if (!result.Succeeded) return BadRequest(result.Errors); //if the user is not created, return the errors

            var roleResult = await _userManager.AddToRoleAsync(user, "Member"); //adds the user to the member role

            if (!roleResult.Succeeded) return BadRequest(result.Errors); //if the user is not added to the member role, return the errors

            return new UserDto //returns the user dto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender

            };
        }

        [HttpPost("login")] // POST: api/account/login
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {

            var user = await _userManager.Users
            .Include(p => p.Photos)
            .SingleOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower()); //checks if the username exists in the database

            if (user == null) return Unauthorized("Invalid username");

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password); //checks if the password is correct

            if (!result) return Unauthorized("Invalid Password"); //if the password is incorrect, return unauthorized


            return new UserDto //returns the user dto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url, //returns the main photo of the user
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };


        }

        private async Task<bool> UserExists(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower()); // checks if the username exists in the database- loops through the users and checks if the username matches the username passed in
        }

    }
}