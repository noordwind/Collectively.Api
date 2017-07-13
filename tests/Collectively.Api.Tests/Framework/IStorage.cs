using Collectively.Common.Types;
using System.Threading.Tasks;

namespace Collectively.Api.Tests.Framework
{
    public interface IStorage
    {
        Task<Maybe<object>> FetchAsync();
        Task SaveAsync(object obj);
    }
}