namespace VsaTemplate.Common.Abstractions;

using Microsoft.AspNetCore.Routing;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
