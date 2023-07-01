

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key; //this is the key that will be used to sign the token
        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])); //gets the token key from the appsettings.json file
        }
        public string CreateToken(AppUser user)
        {
            var claims = new List<Claim> //creates a list of claims- a claim is a statement about the user that is encoded into the token
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()), //adds the username to the claims
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),

            };

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