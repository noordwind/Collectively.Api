using System;
using Coolector.Common.Extensions;

namespace Coolector.Common.Types
{
    internal sealed class CommonResult
    {
        public bool Failure { get; }
        public bool Success => !Failure;
        private readonly string _error;

        public string Error
        {
            get
            {
                if (Success)
                    throw new InvalidOperationException("Success message not provided.");

                return _error;
            }
        }

        public CommonResult(bool failure, string error)
        {
            if (failure)
            {
                if (error.Empty())
                    throw new ArgumentNullException(nameof(error), "Error message for failure not provided.");
            }
            else
            {
                if (error != null)
                    throw new ArgumentException("Invalid error message for success.", nameof(error));
            }

            Failure = failure;
            _error = error;
        }
    }
}