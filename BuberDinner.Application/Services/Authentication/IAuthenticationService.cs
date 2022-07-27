using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentResults;

namespace BuberDinner.Application.Services.Authentication
{
    public interface IAuthenticationService
    {
        Result<AuthenticationResult> Register(string firstName, string lastName, string email, string password);
        Result<AuthenticationResult> Login(string email, string password);
    }
}