using FluentValidation;
using VsaTemplate.Common.Behaviors;
using VsaTemplate.Infrastructure.Configuration;

namespace VsaTemplate.DependencyInjection;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration config
    )
    {
        var luckyPennyOptions = config
            .GetSection(LuckyPennyOptions.SectionName)
            .Get<LuckyPennyOptions>();

        var assembly = typeof(Program).Assembly;
        services.AddMediatR(config =>
        {
            if (
                luckyPennyOptions is not null
                && !string.IsNullOrEmpty(luckyPennyOptions.LicenseKey)
            )
            {
                config.LicenseKey = luckyPennyOptions.LicenseKey;
            }

            config.RegisterServicesFromAssembly(assembly);
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));

            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(assembly);
        return services;
    }
}
