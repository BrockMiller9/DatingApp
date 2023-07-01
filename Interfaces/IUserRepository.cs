using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user); // this method will update the user in the database

        Task<bool> SaveAllAsync(); // this method will save all changes to the database

        Task<IEnumerable<AppUser>> GetUsersAsync(); // this method will get all users from the database
        Task<AppUser> GetUserByIdAsync(int id); // this method will get a user by their id from the database
        Task<AppUser> GetUserByUsernameAsync(string username); // this method will get a user by their username from the database
        Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams); // this method will get all members from the database
        Task<MemberDto> GetMemberAsync(string username); // this method will get a member by their username from the database
    }
}