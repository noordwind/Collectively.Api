using System.Collections.Generic;
using Coolector.Api.Queries;
using Coolector.Common.Types;
using Coolector.Services.Users.Shared.Dto;

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