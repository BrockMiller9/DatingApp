
namespace API.Helpers
{
    public class UserParams : PaginationParams
    {
        // We set the default values for the page number and page size
        // We also set the maximum page size to 50
        // We use a private field for the page size and a public property to get and set the value



        public string CurrentUsername { get; set; }
        public string Gender { get; set; }

        public int MinAge { get; set; } = 18; // this is the minimum age for a user

        public int MaxAge { get; set; } = 150; // this is the maximum age for a user

        public string OrderBy { get; set; } = "lastActive"; // this is the default order by property

    }
}