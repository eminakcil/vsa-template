using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VsaTemplate.Common.Abstractions;
using VsaTemplate.Common.Extensions;
using VsaTemplate.Common.Models;
using VsaTemplate.Infrastructure.Persistence;

namespace VsaTemplate.Features.Auth.RefreshToken;

public record RefreshTokenCommand(string AccessToken, string RefreshToken)
    : IRequest<Result<TokenResponse>>;

public class RefreshTokenHandler(AppDbContext context, IJwtProvider jwtProvider)
    : IRequestHandler<RefreshTokenCommand, Result<TokenResponse>>
{
    public async Task<Result<TokenResponse>> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken
    )
    {
        var principal = jwtProvider.ValidateToken(request.AccessToken);
        if (principal is null)
        {
            return Result<TokenResponse>.Failure(
                Error.Unauthorized("Geçersiz veya bozuk Access Token.")
            );
        }

        var userIdString = principal.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (!Guid.TryParse(userIdString, out var userId))
        {
            return Result<TokenResponse>.Failure(Error.Unauthorized("Token içeriği geçersiz."));
        }

        var user = await context
            .Users.Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user is null)
        {
            return Result<TokenResponse>.Failure(Error.Unauthorized("Kullanıcı bulunamadı."));
        }

        var refreshToken = user.RefreshTokens.FirstOrDefault(rt =>
            rt.Token == request.RefreshToken
        );

        if (refreshToken is null || refreshToken.ExpiryTime <= DateTimeOffset.UtcNow)
        {
            return Result<TokenResponse>.Failure(
                Error.Unauthorized("Oturum süresi dolmuş, lütfen tekrar giriş yapın.")
            );
        }

        var tokenResponse = jwtProvider.Generate(user);

        user.AddRefreshToken(tokenResponse.RefreshToken, DateTimeOffset.UtcNow.AddDays(7));

        await context.SaveChangesAsync(cancellationToken);

        return Result<TokenResponse>.Success(tokenResponse);
    }
}

public class RefreshTokenEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "auth/refresh-token",
                async (RefreshTokenCommand command, ISender sender) =>
                {
                    var result = await sender.Send(command);
                    return result.ToActionResult();
                }
            )
            .WithTags("Authentication");
    }
}
