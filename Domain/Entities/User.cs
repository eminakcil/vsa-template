using VsaTemplate.Common.Abstractions;
using VsaTemplate.Domain.Constants;
using RoleEnum = VsaTemplate.Domain.Constants.Role;

namespace VsaTemplate.Domain.Entities;

public sealed class User : BaseEntity
{
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public RoleEnum Role { get; private set; }

    private readonly List<RefreshToken> _refreshTokens = new();
    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

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

        Id = Guid.CreateVersion7();
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
    }

    public void AddRefreshToken(string token, DateTimeOffset expiryTime)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentException("Refresh token boş olamaz.", nameof(token));

        _refreshTokens.Add(new RefreshToken(Id, token, expiryTime));
    }
}
