using System.Collections.Generic;
using Collectively.Api.Queries;
using Collectively.Common.Types;


namespace Collectively.Api.Filters
{
    public class BrowseUsersFilter : IFilter<UserDto, BrowseUsers>
    {
        public IEnumerable<UserDto> Filter(IEnumerable<UserDto> values, BrowseUsers query)
        {
            return values;
        }
    }
}