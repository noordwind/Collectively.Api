using System.Collections.Generic;
using Coolector.Common.Types;
using Coolector.Dto.Users;

namespace Coolector.Core.Filters
{
    public class BrowseUsersFilter : IFilter<UserDto, BrowseUsers>
    {
        public IEnumerable<UserDto> Filter(IEnumerable<UserDto> values, BrowseUsers query)
        {
            return values;
        }
    }
}