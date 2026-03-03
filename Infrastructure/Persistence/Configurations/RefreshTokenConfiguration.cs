using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VsaTemplate.Domain.Entities;

namespace VsaTemplate.Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(rt => rt.Id);
        builder.Property(rt => rt.Id).ValueGeneratedNever();

        builder.Property(rt => rt.Token).IsRequired().HasMaxLength(255);

        builder.HasIndex(rt => rt.Token).IsUnique();
    }
}
