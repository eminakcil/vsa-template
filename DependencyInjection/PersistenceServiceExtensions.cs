using Microsoft.EntityFrameworkCore;
using VsaTemplate.Infrastructure.Persistence;
using VsaTemplate.Infrastructure.Persistence.Interceptors;

namespace VsaTemplate.DependencyInjection;

public static class DatabaseExtension
{
    public static IServiceCollection AddPersistenceServices(
        this IServiceCollection services,
        IConfiguration config
    )
    {
        services.AddScoped<AuditInterceptor>();
        services.AddScoped<SoftDeleteInterceptor>();

        services.AddDbContext<AppDbContext>(
            (sp, options) =>
            {
                var auditInterceptor = sp.GetRequiredService<AuditInterceptor>();
                var softDeleteInterceptor = sp.GetRequiredService<SoftDeleteInterceptor>();

                options
                    .UseInMemoryDatabase("VsaTemplateDb")
                    .AddInterceptors(auditInterceptor, softDeleteInterceptor);
            }
        );

        return services;
    }
}
