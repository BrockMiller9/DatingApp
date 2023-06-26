
namespace API.Entities
{
    public class AppUser
    {

        //this class is going to represent our user entity inside our database- ie our user table
        public int Id { get; set; }

        
        public string UserName { get; set; }

        public byte[] PasswordHash { get; set; } // byte[] is a byte array

        public byte[] PasswordSalt { get; set; }
    }
}