using System.Collections.Generic;

namespace Collectively.Api.Validation
{
    public interface IValidator<in T>
    {
        IEnumerable<string> SetPropertiesAndValidate(T value);
    }
}