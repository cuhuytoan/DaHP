using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CMS.API.Data;
using CMS.API.Data.Entities;
using CMS.API.Models.DTOs;
using CMS.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CMS.API.Services;

public class TokenService : ITokenService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<TokenService> _logger;

    public TokenService(
        ApplicationDbContext context,
        IConfiguration configuration,
        ILogger<TokenService> logger)
    {
        _context = context;
        _configuration = configuration;
        _logger = logger;
    }

    public string GenerateAccessToken(UserDto user, IList<string> roles)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"] ?? "YourDefaultSecretKeyThatIsAtLeast32CharactersLong!");
        var issuer = _configuration["JwtSettings:Issuer"] ?? "CMS.API";
        var audience = _configuration["JwtSettings:Audience"] ?? "CMS.API";
        var expirationMinutes = int.Parse(_configuration["JwtSettings:ExpirationInMinutes"] ?? "60");

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email ?? ""),
            new Claim(ClaimTypes.Name, user.FullName ?? ""),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id)
        };

        // Add roles
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public async Task<bool> SaveRefreshTokenAsync(string userId, string refreshToken, DateTime expiresAt)
    {
        try
        {
            // Revoke existing tokens
            var existingTokens = await _context.RefreshTokens
                .Where(t => t.UserId == userId && !t.IsRevoked && !t.IsUsed)
                .ToListAsync();

            foreach (var token in existingTokens)
            {
                token.IsRevoked = true;
            }

            // Create new token
            var newToken = new RefreshToken
            {
                UserId = userId,
                Token = refreshToken,
                JwtId = Guid.NewGuid().ToString(),
                IsUsed = false,
                IsRevoked = false,
                CreateDate = DateTime.UtcNow,
                ExpiryDate = expiresAt
            };

            _context.RefreshTokens.Add(newToken);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving refresh token for user {UserId}", userId);
            return false;
        }
    }

    public async Task<bool> ValidateRefreshTokenAsync(string userId, string refreshToken)
    {
        var token = await _context.RefreshTokens
            .FirstOrDefaultAsync(t => 
                t.UserId == userId && 
                t.Token == refreshToken && 
                !t.IsUsed && 
                !t.IsRevoked);

        if (token == null)
        {
            return false;
        }

        if (token.ExpiryDate < DateTime.UtcNow)
        {
            token.IsRevoked = true;
            await _context.SaveChangesAsync();
            return false;
        }

        return true;
    }

    public async Task<bool> RevokeRefreshTokenAsync(string userId)
    {
        var tokens = await _context.RefreshTokens
            .Where(t => t.UserId == userId && !t.IsRevoked)
            .ToListAsync();

        foreach (var token in tokens)
        {
            token.IsRevoked = true;
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RevokeAllRefreshTokensAsync(string userId)
    {
        var tokens = await _context.RefreshTokens
            .Where(t => t.UserId == userId)
            .ToListAsync();

        foreach (var token in tokens)
        {
            token.IsRevoked = true;
            token.IsUsed = true;
        }

        await _context.SaveChangesAsync();
        return true;
    }
}
