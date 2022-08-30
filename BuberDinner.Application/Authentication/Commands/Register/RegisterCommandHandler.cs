using BuberDinner.Application.Common.Interfaces.Auth;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Application.Services.Authentication;
using FluentResults;
using BuberDinner.Domain.Entities;
using BuberDinner.Application.Common.Errors;
using MediatR;

namespace BuberDinner.Application.Authentication.Commands.Register;
public class RegisterCommandHandler :
    IRequestHandler<RegisterCommand, Result<AuthenticationResult>>
{
    public RegisterCommandHandler(IJwtGenerator jwtTokenGenerator, IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }

    private readonly IJwtGenerator _jwtTokenGenerator;

    private readonly IUserRepository _userRepository;

    public async Task<Result<AuthenticationResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (_userRepository.GetUserByEmail(request.Email) is not null)
                return Result.Fail<AuthenticationResult>(new[] { new DuplicateEmailError() });

        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Password = request.Password
        };

        _userRepository.Add(user);

        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }

}