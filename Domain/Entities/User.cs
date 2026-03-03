using VsaTemplate.Common.Abstractions;
using VsaTemplate.Domain.Constants;
using RoleEnum = VsaTemplate.Domain.Constants.Role;

namespace VsaTemplate.Domain.Entities;

public sealed class User : BaseEntity
{
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public RoleEnum Role { get; private set; }

    public string? RefreshToken { get; private set; }
    public DateTimeOffset? RefreshTokenExpiryTime { get; private set; }

    private User()
    {
        Email = null!;
        PasswordHash = null!;
    }

    public User(string email, string passwordHash, RoleEnum role = RoleEnum.User)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentNullException(nameof(email));

        if (!Enum.IsDefined(typeof(RoleEnum), role))
            throw new ArgumentException("Geçersiz rol.", nameof(role));

        Id = Guid.NewGuid();
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
    }

    public void UpdateRefreshToken(string refreshToken, DateTimeOffset expiryTime)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            throw new ArgumentException("Refresh token boş olamaz.", nameof(refreshToken));

        RefreshToken = refreshToken;
        RefreshTokenExpiryTime = expiryTime;
    }
}
