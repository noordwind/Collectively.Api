using System.Collections.Generic;

namespace Coolector.Api.Validation
{
    public interface IValidator<in T>
    {
        IEnumerable<string> SetPropertiesAndValidate(T value);
    }
}