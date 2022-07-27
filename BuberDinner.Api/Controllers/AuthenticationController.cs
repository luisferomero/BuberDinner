using BuberDinner.Application.Common.Errors;
using BuberDinner.Application.Services.Authentication;
using BuberDinner.Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterRequest request)
        {
            var registerResult = _authenticationService.Register(request.FirstName, request.LastName, request.Email, request.Password);

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
        public IActionResult Login(LoginRequest request)
        {
            var loginResult = _authenticationService.Login(request.Email, request.Password);

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
}