using System.Collections.Generic;

namespace Coolector.Api.Validation
{
    public class EmptyValidator<T> : IValidator<T>
    {
        public IEnumerable<string> SetPropertiesAndValidate(T value)
        {
            yield break;
        }
    }
}