using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Common.Errors;
using BuberDinner.Application.Services.Authentication;
using FluentResults;
using FluentValidation;
using MediatR;

namespace BuberDinner.Application.Common.Behaviors;
public class ValidationBehavior<TRequest, TResponse> :
    IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : ResultBase, new()
{
    private readonly IValidator<TRequest> _validator;

    public ValidationBehavior(IValidator<TRequest>? validator = null)
    {
        _validator = validator;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
    {
        if(_validator is null)
            return await next();

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid)
            return await next();

        var errors = validationResult.Errors
            .Select(x => new Error(x.ErrorMessage)
                .WithMetadata(new Dictionary<string, object>
                {
                    {"PropertyName", x.PropertyName},
                    {"ErrorType", ErrorType.Validation}
                })
            );

        return (dynamic)Result.Fail(errors);
    }
}