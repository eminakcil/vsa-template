using MediatR;
using VsaTemplate.Common.Abstractions;
using VsaTemplate.Common.Extensions;
using VsaTemplate.Common.Models;
using RoleEnum = VsaTemplate.Domain.Constants.Role;

namespace VsaTemplate.Features.Auth.Me;

public record GetMeResponse(Guid Id, string Email, RoleEnum Role);

public record GetMeQuery() : IRequest<Result<GetMeResponse>>;

public class GetMeHandler(ICurrentUserService currentUserService)
    : IRequestHandler<GetMeQuery, Result<GetMeResponse>>
{
    public async Task<Result<GetMeResponse>> Handle(
        GetMeQuery request,
        CancellationToken cancellationToken
    )
    {
        var userId = currentUserService.UserId;
        var email = currentUserService.Email;
        var role = currentUserService.Role;

        var response = new GetMeResponse(userId, email, role);

        return await Task.FromResult(Result<GetMeResponse>.Success(response));
    }
}

public class GetMeEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "auth/me",
                async (ISender sender) =>
                {
                    var result = await sender.Send(new GetMeQuery());
                    return result.ToActionResult();
                }
            )
            .WithName("GetMe")
            .WithTags("Authentication")
            .RequireAuthorization();
    }
}
