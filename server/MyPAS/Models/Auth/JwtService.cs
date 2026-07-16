using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyPAS.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class JwtService : IJwtService
{
    
    private readonly string _secretKey = string.Empty;
    
    private readonly string _issuer = string.Empty;
   
    private readonly string _audience = string.Empty;
 
    private readonly int _expiryMinutes = 60;

    public JwtService(IConfiguration config)
    {
        _secretKey = config["JwtSettings:SecretKey"] ?? throw new ArgumentNullException("JwtSettings:SecretKey", "SecretKey cannot be null.");
        _issuer = config["JwtSettings:Issuer"] ?? throw new ArgumentNullException("JwtSettings:Issuer", "Issuer cannot be null.");
        _audience = config["JwtSettings:Audience"] ?? throw new ArgumentNullException("JwtSettings:Audience", "Audience cannot be null.");
        _expiryMinutes = int.TryParse(config["JwtSettings:ExpirationMinutes"], out var expiryMinutes) ? expiryMinutes : throw new ArgumentNullException("JwtSettings:ExpirationMinutes", "ExpirationMinutes must be a valid integer.");
    }

    public string GenerateToken(string userId, string email, string userName, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim("username", userName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_expiryMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
