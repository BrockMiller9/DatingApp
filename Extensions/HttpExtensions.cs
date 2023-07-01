
using System.Text.Json;
using API.Helpers;

namespace API.Extensions
{
    public static class HttpExtensions
    {
        // We add a new extension method to the HttpResponse class
        // This method will add the pagination header to the response
        // We use the JsonSerializer class to serialize the pagination header into JSON
        // We then add the pagination header to the response headers
        // We also add the Access-Control-Expose-Headers header to the response headers
        public static void AddPaginationHeader(this HttpResponse response, PaginationHeader header)
        {
            var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            response.Headers.Add("Pagination", JsonSerializer.Serialize(header, jsonOptions));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}