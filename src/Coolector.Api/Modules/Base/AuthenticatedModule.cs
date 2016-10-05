using Coolector.Common.Commands;
using Coolector.Core.Commands;
using Coolector.Common.Extensions;
using Nancy.Security;
using Nancy.ModelBinding;

namespace Coolector.Api.Modules.Base
{
    public class AuthenticatedModule : ModuleBase
    {
        private string _currentUserId;

        public AuthenticatedModule(ICommandDispatcher commandDispatcher, string modulePath = "")
            : base(commandDispatcher, modulePath)
        {
            this.RequiresAuthentication();
        }

        protected string CurrentUserId
        {
            get
            {
                if (_currentUserId.Empty())
                    SetCurrentUserId(Context.CurrentUser?.Identity?.Name?.Replace("auth0|", string.Empty));

                return _currentUserId;
            }
        }

        protected void SetCurrentUserId(string id)
        {
            _currentUserId = id;
        }

        protected T Bind<T>() where T : new() => BindRequest<T>();

        protected T BindAuthenticatedCommand<T>() where T : IAuthenticatedCommand, new()
        {
            var command = Bind<T>();
            command.UserId = CurrentUserId;

            return command;
        }
    }
}