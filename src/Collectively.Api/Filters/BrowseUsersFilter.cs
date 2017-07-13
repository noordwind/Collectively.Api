using System.Collections.Generic;
using Collectively.Api.Queries;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Users;

namespace Collectively.Api.Filters
{
    public class BrowseUsersFilter : IFilter<User, BrowseUsers>
    {
        public IEnumerable<User> Filter(IEnumerable<User> values, BrowseUsers query)
        {
            return values;
        }
    }
}