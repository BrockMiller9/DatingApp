
using API.Extensions;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class AppUser : IdentityUser<int>
    {

        public DateOnly DateOfBirth { get; set; }

        public string KnownAs { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow; // this is going to be the date that the user is created

        public DateTime LastActive { get; set; } = DateTime.UtcNow; // this is going to be the date that the user is created

        public string Gender { get; set; }

        public string Introduction { get; set; }

        public string LookingFor { get; set; }

        public string Interests { get; set; } // this is going to be a comma separated list of strings

        public string City { get; set; }

        public string Country { get; set; }

        public List<Photo> Photos { get; set; } = new(); // this is going to be a list of photos

        public List<UserLike> LikedByUsers { get; set; } // this is going to be a list of users that like the current user

        public List<UserLike> LikedUsers { get; set; } // this is going to be a list of users that the current user likes

        public List<Message> MessagesSent { get; set; } // this is going to be a list of messages that the current user has sent

        public List<Message> MessagesReceived { get; set; } // this is going to be a list of messages that the current user has received

        public ICollection<AppUserRole> UserRoles { get; set; }
    }


}