namespace API.DTOs
{
    public class PhotoDto
    {
        public int Id { get; set; } // this is going to be the primary key for our photo table

        public string Url { get; set; } // this is going to be the url of the photo

        public bool IsMain { get; set; } // this is going to be a boolean to indicate whether the photo is the main photo for the user

    }
}