using BuberDinner.Domain.Entities;

namespace BuberDinner.Application.Common.Interfaces.Auth
{
    public interface IJwtGenerator
    {
        string GenerateToken(User user);
    }
}