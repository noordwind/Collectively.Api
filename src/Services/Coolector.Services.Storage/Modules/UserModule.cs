using Coolector.Services.Storage.Providers;
using Nancy;

namespace Coolector.Services.Storage.Modules
{
    public class UserModule : NancyModule
    {
        private readonly IUserProvider _userProvider;

        public UserModule(IUserProvider userProvider) : base("users")
        {
            _userProvider = userProvider;
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