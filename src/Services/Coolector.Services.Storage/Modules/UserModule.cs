using Coolector.Services.Storage.Providers;
using Coolector.Services.Storage.Queries;
using Nancy;
using Nancy.ModelBinding;

namespace Coolector.Services.Storage.Modules
{
    public class UserModule : ModuleBase
    {
        private readonly IUserProvider _userProvider;

        public UserModule(IUserProvider userProvider) : base("users")
        {
            _userProvider = userProvider;

            Get("/", async args =>
            {
                var query = this.Bind<BrowseUsers>();
                var users = await _userProvider.BrowseAsync(query);

                return FromPagedResult(users);
            });

            Get("/{id}", async args =>
            {
                var user = await _userProvider.GetAsync((string) args.Id);
                if (user.HasValue)
                    return user.Value;

                return HttpStatusCode.NotFound;
            });
        }
    }
}