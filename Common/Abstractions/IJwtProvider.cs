using VsaTemplate.Common.Entities;

namespace VsaTemplate.Common.Abstractions;

public interface IJwtProvider
{
    string Generate(User user);
}
