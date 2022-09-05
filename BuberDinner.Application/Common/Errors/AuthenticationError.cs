using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentResults;

namespace BuberDinner.Application.Common.Errors;
public class AuthenticationError : IError
{
    public List<IError> Reasons => new List<IError>();

    public string Message => "Email or password are incorrect";

    public Dictionary<string, object> Metadata => new Dictionary<string, object>() { { "ErrorType", ErrorType.Forbidden } };
}