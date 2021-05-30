using System;
using System.Collections.Generic;
using Application.Common.ErrorManagement.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
// ReSharper disable PossibleNullReferenceException

#pragma warning disable 1591

namespace Api.Filters
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger<ApiExceptionFilterAttribute> _logger;
        private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

        public ApiExceptionFilterAttribute(ILogger<ApiExceptionFilterAttribute> logger)
        {
            _logger = logger;
            // Register known exception types and handlers.
            _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                {typeof(NotFoundException), HandleNotFoundException},
                {typeof(ApplicationException), HandleApplicationException},
                {typeof(BusinessException), HandleBusinessException},
                {typeof(AuthenticationException), HandleAuthenticationException},
                {typeof(ConfigurationException), HandleConfigurationException},
            };
        }

        public override void OnException(ExceptionContext context)
        {
            HandleException(context);

            base.OnException(context);
        }

        private void HandleException(ExceptionContext context)
        {
            var type = context.Exception.GetType();

            _logger.LogError(context.Exception, $"Exception of type {type} occurred with message {context.Exception.Message}.");

            if (_exceptionHandlers.ContainsKey(type))
            {
                _exceptionHandlers[type].Invoke(context);
                return;
            }

            if (!context.ModelState.IsValid)
            {
                HandleInvalidModelStateException(context);
                return;
            }

            HandleUnknownException(context);
        }

        private void HandleUnknownException(ExceptionContext context)
        {
            var details = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An error occurred while processing your request.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                Detail = "This is not your fault. We couldn't recover from this error. Please contact staff.",
            };

            context.Result = new ObjectResult(details)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            context.ExceptionHandled = true;
        }

        private void HandleInvalidModelStateException(ExceptionContext context)
        {
            var details = new ValidationProblemDetails(context.ModelState)
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };

            context.Result = new BadRequestObjectResult(details);

            context.ExceptionHandled = true;
        }

        private void HandleNotFoundException(ExceptionContext context)
        {
            var exception = context.Exception as NotFoundException;

            var details = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Title = "The specified resource was not found.",
                Detail = exception.Message
            };

            context.Result = new NotFoundObjectResult(details);

            context.ExceptionHandled = true;
        }

        private void HandleApplicationException(ExceptionContext context)
        {
            var exception = context.Exception as ApplicationException;

            var details = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "An application exception occurred while processing your request.",
                Detail = exception.Message,
                Status = StatusCodes.Status400BadRequest
            };

            context.Result = new BadRequestObjectResult(details);

            context.ExceptionHandled = true;
        }

        private void HandleBusinessException(ExceptionContext context)
        {
            var exception = context.Exception as BusinessException;

            var details = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = exception.Message,
                Detail = exception.GetDetails(),
                Status = StatusCodes.Status400BadRequest
            };

            context.Result = new BadRequestObjectResult(details);

            context.ExceptionHandled = true;
        }

        private void HandleAuthenticationException(ExceptionContext context)
        {
            var exception = context.Exception as AuthenticationException;

            var details = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "An authentication error occurred while processing your request.",
                Detail = exception.Message,
                Status = StatusCodes.Status401Unauthorized
            };

            context.Result = new UnauthorizedObjectResult(details);

            context.ExceptionHandled = true;
        }

        private void HandleConfigurationException(ExceptionContext context)
        {
            var exception = context.Exception as ConfigurationException;

            var details = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                Title = "An error occurred while processing your request.",
                Detail = "This is not your fault. We couldn't recover from this error. Please contact staff.",
                Status = StatusCodes.Status500InternalServerError
            };

            context.Result = new ObjectResult(details);

            _logger.LogError(exception, $"Configuration exception occurred with missing key {exception.GetMissingConfigurationKey}.");

            context.ExceptionHandled = true;
        }
    }
}