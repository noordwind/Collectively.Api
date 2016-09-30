using System.Collections.Generic;
using Coolector.Common.DTO.Users;
using Coolector.Common.Extensions;
using Coolector.Common.Types;

namespace Coolector.Core.Filters
{
    public class BrowseUsersFilter : IFilter<UserDto, BrowseUsers>
    {
        public Maybe<PagedResult<UserDto>> Filter(Maybe<IEnumerable<UserDto>> values, BrowseUsers query)
        {
            if(values.HasNoValue)
                return new Maybe<PagedResult<UserDto>>();

            return values.Value.Paginate(query);
        }
    }
}