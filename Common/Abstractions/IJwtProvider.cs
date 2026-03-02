using VsaTemplate.Common.Entities;

namespace VsaTemplate.Common.Abstractions;

public record TokenResponse(string AccessToken, string RefreshToken);

public interface IJwtProvider
{
    TokenResponse Generate(User user);
    string GenerateRefreshToken();
}
