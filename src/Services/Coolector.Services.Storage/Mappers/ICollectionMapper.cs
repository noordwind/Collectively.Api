using System.Collections.Generic;

namespace Coolector.Services.Storage.Mappers
{
    public interface ICollectionMapper<out T>
    {
        IEnumerable<T> Map(IEnumerable<object> source);
    }
}