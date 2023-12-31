namespace API.DTOs
{
    public class MemberDto
    {
        //this class is going to represent our user entity inside our database- ie our user table
        public int Id { get; set; }


        public string UserName { get; set; }

        public string PhotoUrl { get; set; } // this is going to be the url of the photo

        public int Age { get; set; }

        public string KnownAs { get; set; }

        public DateTime Created { get; set; } // this is going to be the date that the user is created

        public DateTime LastActive { get; set; }// this is going to be the date that the user is created

        public string Gender { get; set; }

        public string Introduction { get; set; }

        public string LookingFor { get; set; }

        public string Interests { get; set; } // this is going to be a comma separated list of strings

        public string City { get; set; }

        public string Country { get; set; }

        public List<PhotoDto> Photos { get; set; } // this is going to be a list of photos


    }
}