

using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class LikesController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly ILikesRepository _likesRepository;
        public LikesController(IUserRepository userRepository, ILikesRepository likesRepository)
        {
            _likesRepository = likesRepository;
            _userRepository = userRepository;

        }

        [HttpPost("{username}")] // this will be the route for this method
        public async Task<ActionResult> AddLike(string username)
        {
            var sourceUserId = User.GetUserId(); // this will get the user id of the current user
            var likedUser = await _userRepository.GetUserByUsernameAsync(username); // this will get the user that the current user wants to like
            var sourceUser = await _likesRepository.GetUserWithLikes(sourceUserId); // this will get the current user with the likes

            if (likedUser == null) return NotFound(); // if the user that the current user wants to like is not found

            if (sourceUser.UserName == username) return BadRequest("You cannot like yourself"); // if the current user tries to like himself

            var userLike = await _likesRepository.GetUserLike(sourceUserId, likedUser.Id); // this will get the like between the current user and the user that the current user wants to like

            if (userLike != null) return BadRequest("You already like this user"); // if the current user already likes the user that he wants to like

            userLike = new UserLike // this will create a new user like
            {
                SourceUserId = sourceUserId,
                TargetUserId = likedUser.Id
            };

            sourceUser.LikedUsers.Add(userLike); // this will add the like to the current user

            if (await _userRepository.SaveAllAsync()) return Ok(); // this will save the changes to the database

            return BadRequest("Failed to like user"); // if the like fails
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<LikeDto>>> GetUserLikes([FromQuery] LikesParams likesParams) // this will get the likes of the current user
        {

            likesParams.UserId = User.GetUserId(); // this will get the user id of the current user
            var users = await _likesRepository.GetUserLikes(likesParams); // this will get the users that the current user has liked

            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages));
            return Ok(users);
        }
    }
}