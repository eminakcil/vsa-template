using RoleEnum = VsaTemplate.Domain.Constants.Role;

namespace VsaTemplate.Common.Abstractions;

public interface ICurrentUserService
{
    Guid UserId { get; }
    string Email { get; }
    public RoleEnum Role { get; }
    bool IsAuthenticated { get; }
}
