using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Authentication.Queries.Login;
using BuberDinner.Application.Common.Errors;
using BuberDinner.Contracts.Authentication;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers;
[Route("auth")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public AuthenticationController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = _mapper.Map<RegisterCommand>(request);

        var registerResult = await _mediator.Send(command);

        if (registerResult.IsSuccess)
            return Ok( _mapper.Map<AuthenticationResponse>(registerResult.Value));

        var firstError = registerResult.Errors[0];

        if (firstError is DuplicateEmailError)
            return Problem(statusCode: StatusCodes.Status409Conflict, detail: firstError.Message);

        return Problem();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var query = _mapper.Map<LoginQuery>(request);
        var loginResult = await _mediator.Send(query);

        if (loginResult.IsSuccess)
            return Ok(_mapper.Map<AuthenticationResponse>(loginResult.Value));

        var firstError = loginResult.Errors[0];

        if (firstError is AuthenticationError)
            return Problem(statusCode: StatusCodes.Status403Forbidden, detail: firstError.Message);

        return Problem();
    }
}