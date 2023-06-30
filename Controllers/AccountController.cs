

using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using API.DTOs;
using Microsoft.EntityFrameworkCore;
using API.Interfaces;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly ITokenService _tokenService;

        private readonly DataContext _context;
        public AccountController(DataContext context, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _context = context;
        }

        [HttpPost("register")] // POST: api/account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)//takes in the username and password and returns the app user
        {
            if (await UserExists(registerDto.Username)) return BadRequest("Username is taken"); //checks if the username exists in the database


            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)), //takes in the password and converts it to a byte array
                PasswordSalt = hmac.Key
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto //returns the user dto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
            };
        }

        [HttpPost("login")] // POST: api/account/login
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {

            var user = await _context.Users
            .Include(p => p.Photos)
            .SingleOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower()); //checks if the username exists in the database

            if (user == null) return Unauthorized("Invalid username");

            using var hmac = new HMACSHA512(user.PasswordSalt); //creates a new instance of the hmac with the password salt

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password)); //computes the hash of the password passed in

            for (int i = 0; i < computedHash.Length; i++) //loops through the computed hash
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password"); //checks if the computed hash matches the password hash
            }

            return new UserDto //returns the user dto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url //returns the main photo of the user
            };


        }

        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower()); // checks if the username exists in the database- loops through the users and checks if the username matches the username passed in
        }

    }
}