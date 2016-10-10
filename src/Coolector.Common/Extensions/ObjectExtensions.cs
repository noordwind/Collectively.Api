using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Coolector.Common.Extensions
{
    public static class ObjectExtensions
    {
        public static void SetPropertyValue<TTarget, TProperty>(this TTarget target,
            Expression<Func<TTarget, TProperty>> memberLamda, object value)
        {
            var memberSelectorExpression = memberLamda.Body as MemberExpression;
            var property = memberSelectorExpression?.Member as PropertyInfo;
            property?.SetValue(target, value, null);
        }
    }
}