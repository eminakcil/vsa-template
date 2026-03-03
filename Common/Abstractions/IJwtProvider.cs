using VsaTemplate.Domain.Entities;

namespace VsaTemplate.Common.Abstractions;

public record TokenResponse(string AccessToken, string RefreshToken);

public interface IJwtProvider
{
    TokenResponse Generate(User user);
    string GenerateRefreshToken();
    System.Security.Claims.ClaimsPrincipal? ValidateToken(string token);
}
