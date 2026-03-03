namespace VsaTemplate.Infrastructure.Configuration;

public record LuckyPennyOptions
{
    public const string SectionName = "LuckyPenny";

    public string LicenseKey { get; init; } = string.Empty;
}
