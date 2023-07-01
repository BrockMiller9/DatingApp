
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class LikesRepository : ILikesRepository
    {
        private readonly DataContext _context;

        public LikesRepository(DataContext context)
        {
            _context = context;

        }
        public async Task<UserLike> GetUserLike(int sourceUserId, int targetUserId)
        {
            return await _context.Likes.FindAsync(sourceUserId, targetUserId); // this will find the like between the source user and the target user
        }

        public async Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams)
        {
            var users = _context.Users.OrderBy(u => u.UserName).AsQueryable(); // this will get all the users from the database and order them by their username
            var likes = _context.Likes.AsQueryable(); // this will get all the likes from the database

            if (likesParams.Predicate == "liked") // if the predicate is liked
            {
                likes = likes.Where(like => like.SourceUserId == likesParams.UserId); // this will get all the likes where the source user id is equal to the user id
                users = likes.Select(like => like.TargetUser); // this will get all the users that the current user has liked
            }

            if (likesParams.Predicate == "likedBy") // if the predicate is likedBy
            {
                likes = likes.Where(like => like.TargetUserId == likesParams.UserId); // this will get all the likes where the target user id is equal to the user id
                users = likes.Select(like => like.SourceUser); // this will get all the users that have liked the current user
            }

            var LikedUsers = users.Select(user => new LikeDto // this will return a list of users that the current user has liked
            {
                UserName = user.UserName,
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.CalculateAge(),
                PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).Url,
                City = user.City,
                Id = user.Id
            });

            return await PagedList<LikeDto>.CreateAsync(LikedUsers, likesParams.PageNumber, likesParams.PageSize); // this will return the list of users that the current user has liked
        }

        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            return await _context.Users
            .Include(x => x.LikedUsers)
            .FirstOrDefaultAsync(x => x.Id == userId); // this will get the user with the likes

        }
    }
}