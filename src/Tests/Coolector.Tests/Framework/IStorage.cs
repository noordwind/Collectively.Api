using System.Threading.Tasks;
using Coolector.Common.Types;

namespace Coolector.Tests.Framework
{
    public interface IStorage
    {
        Task<Maybe<object>> FetchAsync();
        Task SaveAsync(object obj);
    }
}