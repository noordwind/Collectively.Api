using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Coolector.Api.Validation;
using Coolector.Common.Commands;
using Nancy;
using Coolector.Common.Extensions;
using NLog;
using ICommandDispatcher = Coolector.Api.Commands.ICommandDispatcher;

namespace Coolector.Api.Framework
{
    public class CommandRequestHandler<T> where T : ICommand
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ICommandDispatcher _dispatcher;
        private readonly T _command;
        private readonly IResponseFormatter _responseFormatter;
        private readonly IValidatorResolver _validatorResolver;
        private Func<T, object> _responseFunc;
        private Func<T, Task<object>> _asyncResponseFunc;
        private Guid _resourceId;

        public CommandRequestHandler(ICommandDispatcher dispatcher, T command,
            IResponseFormatter responseFormatter,
            IValidatorResolver validatorResolver)
        {
            _dispatcher = dispatcher;
            _command = command;
            _responseFormatter = responseFormatter;
            _validatorResolver = validatorResolver;
        }

        public CommandRequestHandler<T> Set(Action<T> action)
        {
            action(_command);

            return this;
        }

        public CommandRequestHandler<T> SetResourceId(Expression<Func<T, Guid>> memberLamda)
        {
            _resourceId = Guid.NewGuid();
            _command.SetPropertyValue(memberLamda, _resourceId);

            return this;
        }

        public CommandRequestHandler<T> OnSuccess(HttpStatusCode statusCode)
        {
            _responseFunc = x => statusCode;

            return this;
        }

        public CommandRequestHandler<T> OnSuccess(Func<T, object> func)
        {
            _responseFunc = func;

            return this;
        }

        public CommandRequestHandler<T> OnSuccess(Func<T, Task<object>> func)
        {
            _asyncResponseFunc = func;

            return this;
        }

        public CommandRequestHandler<T> OnSuccessCreated(string path) => OnSuccessCreated(c => string.Format(path, _resourceId.ToString("N")));

        public CommandRequestHandler<T> OnSuccessCreated(Func<T, string> func)
        {
            _responseFunc = x => _responseFormatter.AsRedirect(func(_command)).WithStatusCode(201).WithResourceIdHeader(_resourceId);

            return this;
        }

        public CommandRequestHandler<T> OnSuccessRedirect(Func<T, string> func)
        {
            _responseFunc = x => _responseFormatter.AsRedirect(func(_command));

            return this;
        }

        public async Task<object> DispatchAsync()
        {
            var commandName = _command.GetType().Name;
            var validator = _validatorResolver.Resolve<T>();
            Logger.Debug($"Validating command: {commandName}.");
            var errors = validator.SetPropertiesAndValidate(_command).ToArray();
            if (errors.Any())
            {
                Logger.Debug($"Command: {commandName} is invalid. Errors: {errors.AggregateLines()}.");
                throw new ValidatorException(errors);
            }

            Logger.Debug($"Dispatching command: {commandName}.");
            object response = null;
            await _dispatcher.DispatchAsync(_command);

            if (_asyncResponseFunc != null)
            {
                response = await _asyncResponseFunc(_command);
            }
            else if (_responseFunc != null)
            {
                response = _responseFunc(_command);
            }

            return response;
        }
    }
}