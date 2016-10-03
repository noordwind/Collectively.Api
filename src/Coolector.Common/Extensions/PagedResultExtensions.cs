using System;
using System.Collections.Generic;
using System.Linq;
using Coolector.Common.Types;

namespace Coolector.Common.Extensions
{
    public static class PagedResultExtensions
    {
        public static Maybe<PagedResult<TResult>> Select<TSource, TResult>(this Maybe<PagedResult<TSource>> result,
                Func<TSource, TResult> selector)
            => result.HasValue ? result.Value.Select(selector) : new Maybe<PagedResult<TResult>>();

        public static PagedResult<TResult> Select<TSource, TResult>(this PagedResult<TSource> result,
            Func<TSource, TResult> selector)
        {
            var mappedResults = result.Items.Select(selector);

            return PagedResult<TResult>.From(result, mappedResults);
        }
    }
}