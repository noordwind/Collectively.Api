using System;
using System.Threading.Tasks;
using Coolector.Common.Types;

namespace Coolector.Common.Extensions
{
    public static class ResultLeftOperandAsyncExtensions
    {
        public static async Task<Result<K>> OnSuccess<T, K>(this Task<Result<T>> resultTask, Func<T, K> func)
        {
            var result = await resultTask;

            return result.OnSuccess(func);
        }

        public static async Task<Result<T>> OnSuccess<T>(this Task<Result> resultTask, Func<T> func)
        {
            var result = await resultTask;

            return result.OnSuccess(func);
        }

        public static async Task<Result<K>> OnSuccess<T, K>(this Task<Result<T>> resultTask, Func<T, Result<K>> func)
        {
            var result = await resultTask;

            return result.OnSuccess(func);
        }

        public static async Task<Result<T>> OnSuccess<T>(this Task<Result> resultTask, Func<Result<T>> func)
        {
            var result = await resultTask;

            return result.OnSuccess(func);
        }

        public static async Task<Result<K>> OnSuccess<T, K>(this Task<Result<T>> resultTask, Func<Result<K>> func)
        {
            var result = await resultTask;

            return result.OnSuccess(func);
        }

        public static async Task<Result> OnSuccess<T>(this Task<Result<T>> resultTask, Func<T, Result> func)
        {
            var result = await resultTask;

            return result.OnSuccess(func);
        }

        public static async Task<Result> OnSuccess(this Task<Result> resultTask, Func<Result> func)
        {
            var result = await resultTask;

            return result.OnSuccess(func);
        }

        public static async Task<Result<T>> Ensure<T>(this Task<Result<T>> resultTask, Func<T, bool> predicate,
            string errorMessage)
        {
            var result = await resultTask;

            return result.Ensure(predicate, errorMessage);
        }

        public static async Task<Result> Ensure(this Task<Result> resultTask, Func<bool> predicate, string errorMessage)
        {
            var result = await resultTask;

            return result.Ensure(predicate, errorMessage);
        }

        public static async Task<Result<K>> Map<T, K>(this Task<Result<T>> resultTask, Func<T, K> func)
        {
            var result = await resultTask;

            return result.Map(func);
        }

        public static async Task<Result<T>> Map<T>(this Task<Result> resultTask, Func<T> func)
        {
            var result = await resultTask;

            return result.Map(func);
        }

        public static async Task<Result<T>> OnSuccess<T>(this Task<Result<T>> resultTask, Action<T> action)
        {
            var result = await resultTask;

            return result.OnSuccess(action);
        }

        public static async Task<Result> OnSuccess(this Task<Result> resultTask, Action action)
        {
            var result = await resultTask;

            return result.OnSuccess(action);
        }

        public static async Task<T> OnBoth<T>(this Task<Result> resultTask, Func<Result, T> func)
        {
            var result = await resultTask;

            return result.OnBoth(func);
        }

        public static async Task<K> OnBoth<T, K>(this Task<Result<T>> resultTask, Func<Result<T>, K> func)
        {
            var result = await resultTask;

            return result.OnBoth(func);
        }
    }
}