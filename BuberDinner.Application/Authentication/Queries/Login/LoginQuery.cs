using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuberDinner.Application.Services.Authentication;
using FluentResults;
using MediatR;

namespace BuberDinner.Application.Authentication.Queries.Login
{
    public record LoginQuery(string Email, string Password) : IRequest<Result<AuthenticationResult>>;
}