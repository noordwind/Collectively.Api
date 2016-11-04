using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Authentication;
using Coolector.Api.Validation;
using Nancy;
using Nancy.Configuration;
using Nancy.ErrorHandling;
using Nancy.Responses;
using NLog;

namespace Coolector.Api.Framework
{
    public class ErrorResponse : JsonResponse
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private ErrorResponse(ErrorMessage error, INancyEnvironment environment) : base(error, new DefaultJsonSerializer(environment), environment)
        {
        }

        public static ErrorResponse FromException(Exception exception, INancyEnvironment environment)
        {
            Logger.Error(exception);

            var validatorException = exception as ValidatorException;
            if (validatorException != null)
            {
                var validationErrors = validatorException.ValidationErrors;

                return new ErrorResponse(ErrorMessage.FromErrors(validationErrors.ToArray()),
                    environment) { StatusCode = HttpStatusCode.BadRequest };
            }

            var statusCode = HttpStatusCode.InternalServerError;
            if (exception is AuthenticationException)
                statusCode = HttpStatusCode.Unauthorized;
            else if (exception is RouteExecutionEarlyExitException)
                statusCode = HttpStatusCode.Unauthorized;
            else if (exception is ValidationException)
                statusCode = HttpStatusCode.BadRequest;
            else if (exception is ArgumentException)
                statusCode = HttpStatusCode.BadRequest;
            else if (exception is NullReferenceException)
                statusCode = HttpStatusCode.BadRequest;

            var error = ErrorMessage.FromExceptions(exception);
            var response = new ErrorResponse(error, environment) { StatusCode = statusCode };
            return response;
        }

        private class ErrorMessage
        {
            public readonly IEnumerable<ErrorDto> Errors;

            private ErrorMessage(IEnumerable<ErrorDto> errors)
            {
                Errors = errors;
            }

            public static ErrorMessage FromErrors(params string[] errors)
                => new ErrorMessage(errors.Select(CreateErrorDto));

            public static ErrorMessage FromExceptions(params Exception[] exceptions)
                => new ErrorMessage(exceptions.Select(CreateErrorDto));

            private static ErrorDto CreateErrorDto(string error)
            {
                return new ErrorDto
                {
                    Message = error
                };
            }

            private static ErrorDto CreateErrorDto(Exception exception)
            {
                return new ErrorDto
                {
                    Message = exception.Message
                };
            }
        }

        private class ErrorDto
        {
            public string Message { get; set; }
        }
    }
}