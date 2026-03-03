using VsaTemplate.Common.Abstractions;
using VsaTemplate.Common.Constants;

namespace VsaTemplate.Domain.Entities;

public sealed class User : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    public string? RefreshToken { get; set; }
    public DateTimeOffset? RefreshTokenExpiryTime { get; set; }
    public string Role { get; set; } = Roles.User;
}
