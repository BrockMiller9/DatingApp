using System.Security.Claims;

namespace API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user) // this will get the username from the claims principal
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;// this will return the username
        }
    }
}



