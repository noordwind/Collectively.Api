using System;
using Coolector.Common.Types;

namespace Coolector.Common.Extensions
{
    public static class MaybeExtensions
    {
        public static Result<T> ToResult<T>(this Maybe<T> maybe, string errorMessage) where T : class
            => maybe.HasNoValue ? Result.Fail<T>(errorMessage) : Result.Ok(maybe.Value);

        public static T Unwrap<T>(this Maybe<T> maybe, T defaultValue = null) where T : class
            => maybe.Unwrap(x => x, defaultValue);

        public static K Unwrap<T, K>(this Maybe<T> maybe, Func<T, K> selector, K defaultValue = default(K))
            where T : class
            => maybe.HasValue ? selector(maybe.Value) : defaultValue;

        public static Maybe<T> Where<T>(this Maybe<T> maybe, Func<T, bool> predicate) where T : class
        {
            if (maybe.HasNoValue)
                return null;

            return predicate(maybe.Value) ? maybe : null;
        }

        public static Maybe<K> Select<T, K>(this Maybe<T> maybe, Func<T, K> selector) where T : class where K : class
            => maybe.HasNoValue ? null : selector(maybe.Value);

        public static Maybe<K> Select<T, K>(this Maybe<T> maybe, Func<T, Maybe<K>> selector) where T : class
            where K : class
            => maybe.HasNoValue ? null : selector(maybe.Value);

        public static void Execute<T>(this Maybe<T> maybe, Action<T> action) where T : class
        {
            if (maybe.HasNoValue)
                return;

            action(maybe.Value);
        }
    }
}