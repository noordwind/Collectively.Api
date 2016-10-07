using System.Collections.Generic;
using Coolector.Common.Types;
using Nancy;
using Nancy.ModelBinding;
using Newtonsoft.Json;

namespace Coolector.Services.Storage.Modules
{
    public class ModuleBase : NancyModule
    {
        public ModuleBase(string modulePath = "") : base(modulePath)
        { 
        }

        protected T BindRequest<T>() where T : new()
        {
            return Request.Body.Length == 0 && Request.Query == null ? new T() : this.Bind<T>();
        }

        //TODO: Add headers etc.
        protected IEnumerable<T> FromPagedResult<T>(Maybe<PagedResult<T>> result)
        {
            return result.HasValue ? result.Value.Items : new List<T>();
        }
    }
}