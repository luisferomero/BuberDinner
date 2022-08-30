using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Authentication.Queries.Login;
using BuberDinner.Application.Common.Errors;
using BuberDinner.Contracts.Authentication;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers;
[Route("auth")]
[ApiController]
public class AuthenticationController : ControllerBase
{

    private readonly IMediator _mediator;

    public AuthenticationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = new RegisterCommand(request.FirstName, request.LastName, request.Email, request.Password);

        var registerResult = await _mediator.Send(command);

        if (registerResult.IsSuccess)
        return Ok(
            new AuthenticationResponse(
                registerResult.Value.User.Id,
                registerResult.Value.User.FirstName,
                registerResult.Value.User.LastName,
                registerResult.Value.User.Email,
                registerResult.Value.Token
            )
        );

        var firstError = registerResult.Errors[0];

        if (firstError is DuplicateEmailError)
        return Problem(statusCode: StatusCodes.Status409Conflict, detail: firstError.Message);

        return Problem();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var query = new LoginQuery(request.Email, request.Password);
        var loginResult = await _mediator.Send(query);

        if (loginResult.IsSuccess)
        return Ok(
            new AuthenticationResponse(
                loginResult.Value.User.Id,
                loginResult.Value.User.FirstName,
                loginResult.Value.User.LastName,
                loginResult.Value.User.Email,
                loginResult.Value.Token
            )
        );

        var firstError = loginResult.Errors[0];

        if (firstError is AuthenticationError)
        return Problem(statusCode: StatusCodes.Status403Forbidden, detail: firstError.Message);

        return Problem();
    }
}