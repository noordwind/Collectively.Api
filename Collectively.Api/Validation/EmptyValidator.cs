using System.Collections.Generic;

namespace Collectively.Api.Validation
{
    public class EmptyValidator<T> : IValidator<T>
    {
        public IEnumerable<string> SetPropertiesAndValidate(T value)
        {
            yield break;
        }
    }
}