using Collectively.Api.Commands;
using Collectively.Api.Storages;
using Collectively.Api.Validation;
using Collectively.Messages.Commands.Remarks;

namespace Collectively.Api.Modules
{
    public class UserFavoriteModule : ModuleBase
    {
        public UserFavoriteModule(ICommandDispatcher commandDispatcher,
            IValidatorResolver validatorResolver,
            IUserStorage userStorage)
            : base(commandDispatcher, validatorResolver, "account/favorites")
        {
            Put("remarks/{remarkId}", async args => await For<AddFavoriteRemark>()
                .OnSuccessAccepted()
                .DispatchAsync());

            Delete("remarks/{remarkId}", async args => await For<DeleteFavoriteRemark>()
                .OnSuccessAccepted()
                .DispatchAsync());                
        }
    }
}