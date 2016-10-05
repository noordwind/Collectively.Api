using Coolector.Services.Users.Queries;
using Coolector.Services.Users.Services;
using Nancy;

namespace Coolector.Services.Users.Modules
{
    public class UserModule : ModuleBase
    {
        private readonly IUserService _userService;

        public UserModule(IUserService userService) : base("/users")
        {
            _userService = userService;
            Get("/", async args =>
            {
                var query = BindRequest<BrowseUsers>();
                var users = await _userService.BrowseAsync(query.Page, query.Results);

                return FromPagedResult(users);
            });
            Get("/{id}", async args =>
            {
                var user = await _userService.GetAsync((string)args.id);
                if (user.HasValue)
                    return user.Value;

                return HttpStatusCode.NotFound;
            });
        }
    }
}