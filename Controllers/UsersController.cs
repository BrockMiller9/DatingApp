using API.Data;
using Microsoft.AspNetCore.Mvc;
using API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using API.Interfaces;
using AutoMapper;
using API.DTOs;
using System.Security.Claims;
using API.Extensions;
using API.Helpers;

namespace API.Controllers
{

    [Authorize] // this means that the user must be logged in to access this controller
    public class UsersController : BaseApiController
    {

        private readonly IUserRepository _userRepository;
        public IMapper _mapper { get; }
        public IPhotoService _photoService { get; }

        public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
        {
            _photoService = photoService;
            _mapper = mapper;
            _userRepository = userRepository;
        }



        [HttpGet] // essentially we are defining the route here - think of flask routes (app.route('/users')
        public async Task<ActionResult<PagedList<MemberDto>>> GetUsers([FromQuery] UserParams userParams)
        {
            var currentUser = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            userParams.CurrentUsername = currentUser.UserName;

            if (string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = currentUser.Gender == "male" ? "female" : "male";
            }

            var users = await _userRepository.GetMembersAsync(userParams);

            Response.AddPaginationHeader(new PaginationHeader(userParams.PageNumber, userParams.PageSize, users.TotalCount, users.TotalPages));

            return Ok(users);



        }


        [HttpGet("{username}")] // we are defining the route here for one user - think of flask routes (app.route('/users/<username>')

        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            return await _userRepository.GetMemberAsync(username); // this will get a user by their username from the database


        }

        [HttpPut] // this is the route for updating a user
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername()); // this will get the user from the database using the username from the token

            if (user == null) return NotFound(); // if the user is not found in the database then return a 404

            _mapper.Map(memberUpdateDto, user); // this will map the memberUpdateDto to the user

            if (await _userRepository.SaveAllAsync()) return NoContent(); // if the user is updated successfully then return a 204

            return BadRequest("Failed to update user"); // if the user is not updated successfully then return a 400
        }

        [HttpPost("add-photo")] // this is the route for adding a photo
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername()); // this will get the user from the database using the username from the token

            if (user == null) return NotFound(); // if the user is not found in the database then return a 404

            var result = await _photoService.AddPhotoAsync(file); // this will add the photo to the cloudinary service

            if (result.Error != null) return BadRequest(result.Error.Message); // if there is an error then return a 400

            var photo = new Photo // this will create a new photo object
            {
                Url = result.SecureUrl.AbsoluteUri, // this will set the url of the photo to the secure url
                PublicId = result.PublicId // this will set the public id of the photo to the public id
            };

            if (user.Photos.Count == 0) // this will check if the user has any photos
            {
                photo.IsMain = true; // this will set the photo to the main photo
            }

            user.Photos.Add(photo); // this will add the photo to the user

            if (await _userRepository.SaveAllAsync())
            {
                return CreatedAtAction(nameof(GetUser),
                new { username = user.UserName }, _mapper.Map<PhotoDto>(photo));
                // this will return a 201 if the photo is added successfully
            }

            return BadRequest("Problem adding photo"); // this will return a 400 if the photo is not added successfully
        }

        [HttpPut("set-main-photo/{photoId}")] // this is the route for setting the main photo
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername()); // this will get the user from the database using the username from the token

            if (user == null) return NotFound();

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId); // this will get the photo from the user

            if (photo == null) return NotFound();

            if (photo.IsMain) return BadRequest("This is already your main photo");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain); // this will get the current main photo
            if (currentMain != null) currentMain.IsMain = false; // this will set the current main photo to false
            photo.IsMain = true; // this will set the photo to the main photo

            if (await _userRepository.SaveAllAsync()) return NoContent(); // this will return a 204 if the photo is set to the main photo

            return BadRequest("Failed to set main photo"); // this will return a 400 if the photo is not set to the main photo
        }

        [HttpDelete("delete-photo/{photoId}")] // this is the route for deleting a photo
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername()); // this will get the user from the database using the username from the token

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId); // this will get the photo from the user

            if (photo == null) return NotFound(); // this will return a 404 if the photo is not found

            if (photo.IsMain) return BadRequest("You cannot delete your main photo"); // this will return a 400 if the photo is the main photo

            if (photo.PublicId != null) // this will check if the photo has a public id- these are the photos we have seeded
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId); // this will delete the photo from the cloudinary service

                if (result.Error != null) return BadRequest(result.Error.Message); // this will return a 400 if there is an error
            }

            user.Photos.Remove(photo); // this will remove the photo from the user
            if (await _userRepository.SaveAllAsync()) return Ok(); // this will return a 200 if the photo is deleted successfully

            return BadRequest("Failed to delete the photo"); // this will return a 400 if the photo is not deleted successfully
        }
    }
}