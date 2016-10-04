using System.Collections.Generic;
using Coolector.Common.Types;
using Nancy;

namespace Coolector.Services.Remarks.Modules
{
    public class ModuleBase : NancyModule
    {
        public ModuleBase(string modulePath = "") : base(modulePath)
        { 
        }

        //TODO: Add headers etc.
        protected IEnumerable<T> FromPagedResult<T>(Maybe<PagedResult<T>> result)
        {
            return result.HasValue ? result.Value.Items : new List<T>();
        }
    }
}