using System.Collections.Generic;
using Coolector.Common.Commands.Remarks;
using Coolector.Common.Extensions;

namespace Coolector.Api.Validation
{
    public class CreateRemarkValidator : IValidator<CreateRemark>
    {
        public IEnumerable<string> SetPropertiesAndValidate(CreateRemark value)
        {
            if (value.UserId.Empty())
                yield return "User was not provided.";
            if (value.Category.Empty())
                yield return "Category was not provided.";
            if (value.Description?.Length > 500)
                yield return "Description is too long (over 500 characters).";
        }
    }
}