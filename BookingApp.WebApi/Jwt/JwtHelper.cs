using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ApiProject.Jwt;

public static class JwtHelper
{
    public static string GenerateJwtToken(JwtDto jwtDto)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtDto.SecretKey));
        
        //Credentials
        var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(JwtClaimNames.Id, jwtDto.Id.ToString()),
            new Claim(JwtClaimNames.Email, jwtDto.Email),
            new Claim(JwtClaimNames.FirstName, jwtDto.FirstName),
            new Claim(JwtClaimNames.LastName, jwtDto.LastName),
            new Claim(JwtClaimNames.UserType, jwtDto.UserType.ToString()),

            new Claim(ClaimTypes.Role, jwtDto.UserType.ToString())
        };
        var expirationTime = DateTime.UtcNow.AddMinutes(jwtDto.ExpirationMinutes);
        var tokenDescriptor = new JwtSecurityToken(jwtDto.Issuer, jwtDto.Audience, claims, null, expirationTime, credentials);
        var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        return token;
    }
}