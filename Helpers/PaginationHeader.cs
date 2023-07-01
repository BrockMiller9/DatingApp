
namespace API.Helpers
{
    public class PaginationHeader
    {
        // We add a constructor to the PaginationHeader class
        // We use this constructor to set the values for the properties
        // We use the JsonPropertyName attribute to change the name of the properties when they are serialized to JSON
        // We use the JsonConstructor attribute to tell the serializer to use this constructor when deserializing the JSON

        public PaginationHeader(int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            CurrentPage = currentPage;
            ItemsPerPage = itemsPerPage;
            TotalItems = totalItems;
            TotalPages = totalPages;
        }

        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
    }
}