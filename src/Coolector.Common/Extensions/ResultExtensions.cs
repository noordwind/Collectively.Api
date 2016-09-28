using System;
using Coolector.Common.Types;

namespace Coolector.Common.Extensions
{
    public static class ResultExtensions
    {
        public static Result<K> OnSuccess<T, K>(this Result<T> result, Func<T, K> func)
            => result.Failure ? Result.Fail<K>(result.Error) : Result.Ok(func(result.Value));

        public static Result<T> OnSuccess<T>(this Result result, Func<T> func)
            => result.IsFailure ? Result.Fail<T>(result.Error) : Result.Ok(func());

        public static Result<K> OnSuccess<T, K>(this Result<T> result, Func<T, Result<K>> func)
            => result.Failure ? Result.Fail<K>(result.Error) : func(result.Value);

        public static Result<T> OnSuccess<T>(this Result result, Func<Result<T>> func)
            => result.IsFailure ? Result.Fail<T>(result.Error) : func();

        public static Result<K> OnSuccess<T, K>(this Result<T> result, Func<Result<K>> func)
            => result.Failure ? Result.Fail<K>(result.Error) : func();

        public static Result OnSuccess<T>(this Result<T> result, Func<T, Result> func)
            => result.Failure ? Result.Fail(result.Error) : func(result.Value);

        public static Result OnSuccess(this Result result, Func<Result> func)
            => result.IsFailure ? result : func();

        public static Result<T> Ensure<T>(this Result<T> result, Func<T, bool> predicate, string errorMessage)
        {
            if (result.Failure)
                return Result.Fail<T>(result.Error);

            return !predicate(result.Value) ? Result.Fail<T>(errorMessage) : Result.Ok(result.Value);
        }

        public static Result Ensure(this Result result, Func<bool> predicate, string errorMessage)
        {
            if (result.IsFailure)
                return Result.Fail(result.Error);

            return !predicate() ? Result.Fail(errorMessage) : Result.Ok();
        }

        public static Result<K> Map<T, K>(this Result<T> result, Func<T, K> func)
            => result.Failure ? Result.Fail<K>(result.Error) : Result.Ok(func(result.Value));

        public static Result<T> Map<T>(this Result result, Func<T> func)
            => result.IsFailure ? Result.Fail<T>(result.Error) : Result.Ok(func());

        public static Result<T> OnSuccess<T>(this Result<T> result, Action<T> action)
        {
            if (result.Success)
                action(result.Value);

            return result;
        }

        public static Result OnSuccess(this Result result, Action action)
        {
            if (result.IsSuccess)
                action();

            return result;
        }

        public static T OnBoth<T>(this Result result, Func<Result, T> func) => func(result);

        public static K OnBoth<T, K>(this Result<T> result, Func<Result<T>, K> func) => func(result);
    }
}