using Microsoft.EntityFrameworkCore;
using VsaTemplate.Infrastructure.Persistence;
using VsaTemplate.Infrastructure.Persistence.Interceptors;

namespace VsaTemplate.DependencyInjection;

public static class PersistenceServiceExtensions
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
                    .UseNpgsql(config.GetConnectionString("DefaultConnection"))
                    .AddInterceptors(softDeleteInterceptor, auditInterceptor);
            }
        );

        return services;
    }

    public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        using AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        context.Database.Migrate();

        return app;
    }
}
