using Coolector.Common.Types;
using System.Threading.Tasks;

namespace Coolector.Api.Tests.Framework
{
    public interface IStorage
    {
        Task<Maybe<object>> FetchAsync();
        Task SaveAsync(object obj);
    }
}