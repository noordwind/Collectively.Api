using Coolector.Dto.Users;
using Coolector.Services.Storage.Providers;
using Coolector.Services.Storage.Queries;

namespace Coolector.Services.Storage.Modules
{
    public class UserModule : ModuleBase
    {
        public UserModule(IUserProvider userProvider) : base("users")
        {
            Get("", async args => await FetchCollection<BrowseUsers, UserDto>
                (async x => await userProvider.BrowseAsync(x)).HandleAsync());

            Get("{id}", async args => await Fetch<GetUser, UserDto>
                (async x => await userProvider.GetAsync(x.Id)).HandleAsync());

            Get("{name}/account", async args => await Fetch<GetUserByName, UserDto>
                (async x => await userProvider.GetByNameAsync(x.Name)).HandleAsync());
        }
    }
}