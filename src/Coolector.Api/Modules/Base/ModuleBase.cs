using System.Collections.Generic;
using Coolector.Common.Commands;
using Coolector.Common.Extensions;
using Coolector.Common.Types;
using Coolector.Core.Commands;
using Nancy;
using Nancy.ModelBinding;

namespace Coolector.Api.Modules.Base
{
    public class ModuleBase : NancyModule
    {
        protected readonly ICommandDispatcher CommandDispatcher;
        private string _currentUserId;

        public ModuleBase(ICommandDispatcher commandDispatcher, string modulePath = "")
            :base(modulePath)
        {
            CommandDispatcher = commandDispatcher;
        }

        protected T BindRequest<T>() where T : new()
        {
            return Request.Body.Length == 0 ? new T() : this.Bind<T>();
        }

        //TODO: Add headers etc.
        protected IEnumerable<T> FromPagedResult<T>(Maybe<PagedResult<T>> result)
        {
            return result.HasValue ? result.Value.Items : new List<T>();
        }

        protected string CurrentUserId
        {
            get
            {
                if (_currentUserId.Empty())
                    SetCurrentUserId(Context.CurrentUser?.Identity?.Name);

                return _currentUserId;
            }
        }

        protected void SetCurrentUserId(string id)
        {
            _currentUserId = id;
        }

        protected T BindAuthenticatedCommand<T>() where T : IAuthenticatedCommand, new()
        {
            var command = BindRequest<T>();
            command.UserId = CurrentUserId;

            return command;
        }
    }
}