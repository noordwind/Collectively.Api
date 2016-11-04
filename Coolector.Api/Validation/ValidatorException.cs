using System;
using System.Collections.Generic;

namespace Coolector.Api.Validation
{
    public class ValidatorException : Exception
    {
        public IEnumerable<string> ValidationErrors { get; }

        public ValidatorException()
        {
        }

        public ValidatorException(string message) : base(message)
        {
        }

        public ValidatorException(params string[] validationErrors) : this("Validation errors occured.", validationErrors)
        {
        }

        public ValidatorException(string message, params string[] validationErrors) : base(message)
        {
            ValidationErrors = validationErrors;
        }
    }
}