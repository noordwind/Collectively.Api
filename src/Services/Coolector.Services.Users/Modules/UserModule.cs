using System.Collections.Generic;
using Coolector.Services.Users.Queries;
using Coolector.Services.Users.Services;
using Nancy;
using Nancy.ModelBinding;

namespace Coolector.Services.Users.Modules
{
    public class UserModule : NancyModule
    {
        private readonly IUserService _userService;

        public UserModule(IUserService userService) : base("/users")
        {
            _userService = userService;
            Get("/", async args =>
            {
                var query = this.Bind<BrowseUsers>();
                var users = await _userService.BrowseAsync(query.Page, query.Results);
                if (users.HasValue)
                    return users.Value.Items;

                return new List<string>();
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