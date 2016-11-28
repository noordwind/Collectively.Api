using System.Collections.Generic;
using Coolector.Api.Queries;
using Coolector.Common.Dto.Users;
using Coolector.Common.Types;

namespace Coolector.Api.Filters
{
    public class BrowseUsersFilter : IFilter<UserDto, BrowseUsers>
    {
        public IEnumerable<UserDto> Filter(IEnumerable<UserDto> values, BrowseUsers query)
        {
            return values;
        }
    }
}