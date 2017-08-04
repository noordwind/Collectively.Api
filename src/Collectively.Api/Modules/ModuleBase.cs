using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Collectively.Api.Commands;
using Collectively.Api.Framework;
using Collectively.Api.Validation;
using Collectively.Messages.Commands;
using Collectively.Common.Extensions;
using Collectively.Common.Nancy;
using Collectively.Common.Queries;
using Collectively.Common.Types;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses;
using Nancy.Security;
using NLog;
using Structure.Sketching.ExtensionMethods;
using Collectively.Common.Security;
using System.Security.Claims;
using System.Security.Authentication;

namespace Collectively.Api.Modules
{
    public abstract class ModuleBase : NancyModule
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        protected readonly ICommandDispatcher CommandDispatcher;
        private readonly IValidatorResolver _validatorResolver;
        private string _currentUserId;

        protected ModuleBase(ICommandDispatcher commandDispatcher,
            IValidatorResolver validatorResolver,
            string modulePath = "")
            : base(modulePath)
        {
            CommandDispatcher = commandDispatcher;
            _validatorResolver = validatorResolver;
        }

        protected CommandRequestHandler<T> For<T>() where T : ICommand, new()
        {
            var command = BindRequest<T>();
            var authenticatedCommand = command as IAuthenticatedCommand;
            if (authenticatedCommand == null)
            {
                return new CommandRequestHandler<T>(CommandDispatcher, command, Response,
                    _validatorResolver, Negotiate, CreateRequest<T>());
            }

            this.RequiresAuthentication();
            authenticatedCommand.UserId = CurrentUserId;

            return new CommandRequestHandler<T>(CommandDispatcher, command, Response,
                _validatorResolver,Negotiate, CreateRequest<T>());
        }

        protected Collectively.Messages.Commands.Models.File ToFile()
        {
            var files = Context.Request.Files;
            var file = files?.FirstOrDefault();
            if (file != null)
            {
                using(var stream = new MemoryStream())
                {
                    file.Value.CopyTo(stream);
                    var bytes = stream.ToArray();

                    return new Collectively.Messages.Commands.Models.File
                    {
                        Name = file.Name,
                        ContentType = file.ContentType,
                        Base64 = Convert.ToBase64String(bytes)
                    };
                }
            } 
            return null;           
        }

        protected FetchRequestHandler<TQuery, TResult> Fetch<TQuery, TResult>(Func<TQuery, Task<Maybe<TResult>>> fetch)
            where TQuery : IQuery, new() where TResult : class
        {
            var query = BindRequest<TQuery>();
            var authenticatedQuery = query as IAuthenticatedQuery;
            if (authenticatedQuery == null)
                return new FetchRequestHandler<TQuery, TResult>(query, fetch, Negotiate, Request.Url);

            this.RequiresAuthentication();
            authenticatedQuery.UserId = CurrentUserId;

            return new FetchRequestHandler<TQuery, TResult>(query, fetch, Negotiate, Request.Url);
        }

        private CollectivelyIdentity Identity => Context.CurrentUser != null ? 
            Context.CurrentUser as CollectivelyIdentity :
            new CollectivelyIdentity(string.Empty, string.Empty, "active", Enumerable.Empty<Claim>());

        protected FetchRequestHandler<TQuery, TResult> FetchCollection<TQuery, TResult>(
            Func<TQuery, Task<Maybe<PagedResult<TResult>>>> fetch)
            where TQuery : IPagedQuery, new() where TResult : class
        {
            var query = BindRequest<TQuery>();
            var authenticatedQuery = query as IAuthenticatedPagedQuery;
            if (authenticatedQuery == null)
                return new FetchRequestHandler<TQuery, TResult>(query, fetch, Negotiate, Request.Url);

            this.RequiresAuthentication();
            authenticatedQuery.UserId = CurrentUserId;

            return new FetchRequestHandler<TQuery, TResult>(query, fetch, Negotiate, Request.Url);
        }

        protected T BindRequest<T>() where T : new()
        => Request.Body.Length == 0 && Request.Query == null
            ? new T()
            : this.Bind<T>(new BindingConfig(), blacklistedProperties: "UserId");

        protected string CurrentUserId
        {
            get
            {
                if (_currentUserId.Empty())
                {
                    _currentUserId = Context.CurrentUser?.Identity?.Name;
                }

                return _currentUserId;
            }
        }

        protected Response FromStream(Maybe<Stream> stream, string fileName, string contentType)
        {
            if (stream.HasNoValue)
            {
                Logger.Warn($"Stream result has no value, fileName: {fileName}, contentType: {contentType}");
                return HttpStatusCode.NotFound;
            }
            Logger.Debug($"File received successfully, fileName: {fileName}, contentType: {contentType}");
            var response = new StreamResponse(() => stream.Value, contentType);

            return response.AsAttachment(fileName);
        }

        protected string Culture 
        {
            get 
            {
                var culture = Request.Headers.AcceptLanguage?.FirstOrDefault()?.Item1;

                return culture.Empty() ? "en-gb" : culture.TrimToLower();
            }
        }

        protected Messages.Commands.Request CreateRequest<T>()
            => Messages.Commands.Request.Create<T>(Guid.NewGuid(), Request.Url.Path, Culture);
    }
}