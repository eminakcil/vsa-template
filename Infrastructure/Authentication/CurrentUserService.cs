using System.Security.Claims;
using VsaTemplate.Common.Abstractions;
using RoleEnum = VsaTemplate.Domain.Constants.Role;

namespace VsaTemplate.Infrastructure.Authentication;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public Guid UserId
    {
        get
        {
            var userIdString = httpContextAccessor.HttpContext?.User?.FindFirstValue(
                ClaimTypes.NameIdentifier
            );
            return Guid.TryParse(userIdString, out var userId) ? userId : Guid.Empty;
        }
    }

    public string Email =>
        httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email) ?? string.Empty;

    public bool IsAuthenticated =>
        httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    public RoleEnum Role
    {
        get
        {
            var roleClaim = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);

            return Enum.TryParse(roleClaim, out RoleEnum result) ? result : RoleEnum.User;
        }
    }
}
