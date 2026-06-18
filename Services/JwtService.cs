using Assignment2.Models;
using Assignment2.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class JwtService: IJwtService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<ApplicationUser> _userManager;

    public JwtService(
        IConfiguration configuration,
        UserManager<ApplicationUser> userManager)
    {
        _configuration = configuration;
        _userManager = userManager;
    }

    public async Task<string> GenerateTokenAsync(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),

            new Claim(ClaimTypes.Name, user.UserName),

            new Claim(ClaimTypes.Email, user.Email)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role)); // add each role as a claim
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])); // create a symmetric security key using the secret key from configuration// it is used to sign the token and should be kept secret and secure to prevent unauthorized access to the token's contents

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); // create signing credentials using the symmetric security key and HMAC SHA256 algorithm// they are used to sign the token to ensure its integrity and authenticity

        var token = new JwtSecurityToken(
                issuer:
                    _configuration["Jwt:Issuer"],
                audience:
                    _configuration["Jwt:Audience"],
                claims:
                    claims,
                expires:
                    DateTime.UtcNow.AddHours(2),
                signingCredentials:
                    creds);

        return new JwtSecurityTokenHandler().WriteToken(token); // convert obj type token to string // why to convert to string? because the token is sent to the client as a string in the response and is used by the client to authenticate subsequent requests to the server. The client typically includes the token in the Authorization header of HTTP requests, and the server validates the token to ensure that the request is authenticated and authorized to access protected resources.
    }
}