using System;
using System.Linq;

namespace Coolector.Common.Types
{
    public struct Result
    {
        private static readonly Result OkResult = new Result(false, null);
        private readonly CommonResult _commonResult;
        public bool IsFailure => _commonResult.Failure;
        public bool IsSuccess => _commonResult.Success;
        public string Error => _commonResult.Error;

        private Result(bool isFailure, string error)
        {
            _commonResult = new CommonResult(isFailure, error);
        }

        public static Result Ok()
        {
            return OkResult;
        }

        public static Result Fail(string error)
        {
            return new Result(true, error);
        }

        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(false, value, null);
        }

        public static Result<T> Fail<T>(string error)
        {
            return new Result<T>(true, default(T), error);
        }

        public static Result FirstFailureOrSuccess(params Result[] results)
        {
            foreach (Result result in results)
            {
                if (result.IsFailure)
                    return Fail(result.Error);
            }

            return Ok();
        }

        public static Result Combine(string errorMessagesSeparator, params Result[] results)
        {
            var failedResults = results.Where(x => x.IsFailure).ToList();
            if (!failedResults.Any())
                return Ok();

            var errorMessage = string.Join(errorMessagesSeparator,
                failedResults.Select(x => x.Error)
                    .ToArray());

            return Fail(errorMessage);
        }

        public static Result Combine(params Result[] results)
        {
            return Combine(", ", results);
        }
    }

    public struct Result<T>
    {
        private readonly CommonResult _commonResult;
        public bool Failure => _commonResult.Failure;
        public bool Success => _commonResult.Success;
        public string Error => _commonResult.Error;
        private readonly T _value;

        public T Value
        {
            get
            {
                if (!Success)
                    throw new InvalidOperationException("Failure value not provided.");

                return _value;
            }
        }

        internal Result(bool isFailure, T value, string error)
        {
            if (!isFailure && value == null)
                throw new ArgumentNullException(nameof(value));

            _commonResult = new CommonResult(isFailure, error);
            _value = value;
        }

        public static implicit operator Result(Result<T> result)
            => result.Success ? Result.Ok() : Result.Fail(result.Error);
    }
}