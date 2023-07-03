

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key; //this is the key that will be used to sign the token
        private readonly UserManager<AppUser> _userManager;
        public TokenService(IConfiguration config, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])); //gets the token key from the appsettings.json file
        }
        public async Task<string> CreateToken(AppUser user)
        {
            var claims = new List<Claim> //creates a list of claims- a claim is a statement about the user that is encoded into the token
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()), //adds the username to the claims
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),

            };

            var roles = await _userManager.GetRolesAsync(user); //gets the roles of the user

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role))); //adds the roles to the claims

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature); //creates the credentials that will be used to sign the token

            var tokenDescriptor = new SecurityTokenDescriptor //creates the token descriptor- a token descriptor is used to create the token
            {
                Subject = new ClaimsIdentity(claims), //adds the claims to the token descriptor
                Expires = DateTime.UtcNow.AddDays(7), //sets the expiry date of the token
                SigningCredentials = creds //adds the credentials to the token descriptor
            };

            var tokenHandler = new JwtSecurityTokenHandler(); //creates a new instance of the JwtSecurityTokenHandler

            var token = tokenHandler.CreateToken(tokenDescriptor); //creates the token

            return tokenHandler.WriteToken(token); //returns the token
        }
    }
}