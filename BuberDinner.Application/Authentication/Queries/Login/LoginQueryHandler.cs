using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuberDinner.Application.Common.Errors;
using BuberDinner.Application.Common.Interfaces.Auth;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Application.Services.Authentication;
using BuberDinner.Domain.Entities;
using FluentResults;
using MediatR;

namespace BuberDinner.Application.Authentication.Queries.Login;
public class LoginQueryHandler : IRequestHandler<LoginQuery, Result<AuthenticationResult>>
{

    public LoginQueryHandler(IJwtGenerator jwtTokenGenerator, IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }

    private readonly IJwtGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;
    public async Task<Result<AuthenticationResult>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        if (_userRepository.GetUserByEmail(request.Email) is not User user || user.Password != request.Password)
            return Result.Fail<AuthenticationResult>(new AuthenticationError());

        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }
}