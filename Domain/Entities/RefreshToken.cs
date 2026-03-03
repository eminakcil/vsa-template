using VsaTemplate.Common.Abstractions;

namespace VsaTemplate.Domain.Entities;

public sealed class RefreshToken : BaseEntity
{
    public Guid UserId { get; private set; }
    public string Token { get; private set; }
    public DateTimeOffset ExpiryTime { get; private set; }

    public User User { get; private set; } = null!;

    private RefreshToken()
    {
        Token = null!;
    }

    public RefreshToken(Guid userId, string token, DateTimeOffset expiryTime)
    {
        Id = Guid.CreateVersion7();
        UserId = userId;
        Token = token;
        ExpiryTime = expiryTime;
    }
}
