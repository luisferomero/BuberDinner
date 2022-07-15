namespace BuberDinner.Application.Common.Interfaces.Auth
{
    public interface IJwtGenerator
    {
        string GenerateToken(Guid userId, string firstName, string lastName);
    }
}