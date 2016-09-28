using System;
using System.Threading.Tasks;
using Coolector.Common.Types;

namespace Coolector.Common.Extensions
{
    public static class ResultBothOperandsAsyncExtensions
    {
        public static async Task<Result<K>> OnSuccess<T, K>(this Task<Result<T>> resultTask, Func<T, Task<K>> func)
        {
            var result = await resultTask;

            if (result.Failure)
                return Result.Fail<K>(result.Error);

            var value = await func(result.Value);

            return Result.Ok(value);
        }

        public static async Task<Result<T>> OnSuccess<T>(this Task<Result> resultTask, Func<Task<T>> func)
        {
            var result = await resultTask;

            if (result.IsFailure)
                return Result.Fail<T>(result.Error);

            var value = await func();

            return Result.Ok(value);
        }

        public static async Task<Result<K>> OnSuccess<T, K>(this Task<Result<T>> resultTask,
            Func<T, Task<Result<K>>> func)
        {
            var result = await resultTask;

            if (result.Failure)
                return Result.Fail<K>(result.Error);

            return await func(result.Value);
        }

        public static async Task<Result<T>> OnSuccess<T>(this Task<Result> resultTask, Func<Task<Result<T>>> func)
        {
            var result = await resultTask;

            if (result.IsFailure)
                return Result.Fail<T>(result.Error);

            return await func();
        }

        public static async Task<Result<K>> OnSuccess<T, K>(this Task<Result<T>> resultTask, Func<Task<Result<K>>> func)
        {
            var result = await resultTask;

            if (result.Failure)
                return Result.Fail<K>(result.Error);

            return await func();
        }

        public static async Task<Result> OnSuccess<T>(this Task<Result<T>> resultTask, Func<T, Task<Result>> func)
        {
            var result = await resultTask;

            if (result.Failure)
                return Result.Fail(result.Error);

            return await func(result.Value);
        }

        public static async Task<Result> OnSuccess(this Task<Result> resultTask, Func<Task<Result>> func)
        {
            var result = await resultTask;

            if (result.IsFailure)
                return result;

            return await func();
        }

        public static async Task<Result<T>> Ensure<T>(this Task<Result<T>> resultTask, Func<T, Task<bool>> predicate,
            string errorMessage)
        {
            var result = await resultTask;

            if (result.Failure)
                return Result.Fail<T>(result.Error);

            if (!await predicate(result.Value))
                return Result.Fail<T>(errorMessage);

            return Result.Ok(result.Value);
        }

        public static async Task<Result> Ensure(this Task<Result> resultTask, Func<Task<bool>> predicate,
            string errorMessage)
        {
            var result = await resultTask;

            if (result.IsFailure)
                return Result.Fail(result.Error);

            if (!await predicate())
                return Result.Fail(errorMessage);

            return Result.Ok();
        }

        public static async Task<Result<K>> Map<T, K>(this Task<Result<T>> resultTask, Func<T, Task<K>> func)
        {
            var result = await resultTask;

            if (result.Failure)
                return Result.Fail<K>(result.Error);

            var value = await func(result.Value);

            return Result.Ok(value);
        }

        public static async Task<Result<T>> Map<T>(this Task<Result> resultTask, Func<Task<T>> func)
        {
            var result = await resultTask;

            if (result.IsFailure)
                return Result.Fail<T>(result.Error);

            var value = await func();

            return Result.Ok(value);
        }

        public static async Task<Result<T>> OnSuccess<T>(this Task<Result<T>> resultTask, Func<T, Task> action)
        {
            var result = await resultTask;

            if (result.Success)
                await action(result.Value);

            return result;
        }

        public static async Task<Result> OnSuccess(this Task<Result> resultTask, Func<Task> action)
        {
            var result = await resultTask;

            if (result.IsSuccess)
                await action();

            return result;
        }

        public static async Task<T> OnBoth<T>(this Task<Result> resultTask, Func<Result, Task<T>> func)
        {
            var result = await resultTask;

            return await func(result);
        }

        public static async Task<K> OnBoth<T, K>(this Task<Result<T>> resultTask, Func<Result<T>, Task<K>> func)
        {
            var result = await resultTask;

            return await func(result);
        }
    }
}