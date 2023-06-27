
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("Photos")]
    public class Photo
    {

        public int Id { get; set; } // this is going to be the primary key for our photo table

        public string Url { get; set; } // this is going to be the url of the photo

        public bool IsMain { get; set; } // this is going to be a boolean to indicate whether the photo is the main photo for the user

        public string PublicId { get; set; } // this is going to be the public id for the photo 

        public int AppUserId { get; set; } // this is going to be the foreign key for the user

        public AppUser AppUser { get; set; }
    }
}