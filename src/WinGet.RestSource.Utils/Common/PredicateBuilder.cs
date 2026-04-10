// -----------------------------------------------------------------------
// <copyright file="PredicateBuilder.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Enables the efficient, dynamic composition of query predicates.
    /// </summary>
    public static class PredicateBuilder
    {
        /// <summary>
        /// Creates a predicate that evaluates to true.
        /// </summary>
        /// <typeparam name="T">The expression type.</typeparam>
        /// <returns>A constant predicate which always returns true.</returns>
        public static Expression<Func<T, bool>> True<T>() => param => true;

        /// <summary>
        /// Creates a predicate that evaluates to false.
        /// </summary>
        /// <typeparam name="T">The expression type.</typeparam>
        /// <returns>A constant predicate which always returns true.</returns>
        public static Expression<Func<T, bool>> False<T>() => param => false;

        /// <summary>
        /// Combines the first predicate with the second using the logical "and".
        /// </summary>
        /// <typeparam name="T">The expression type.</typeparam>
        /// <param name="first">The base expression.</param>
        /// <param name="second">The expression to combine onto the base.</param>
        /// <returns>A new AND-combined predicate.</returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.IsStarted() ? first.Compose(second, Expression.AndAlso) : second;
        }

        /// <summary>
        /// Combines the first predicate with the second using the logical "or".
        /// </summary>
        /// <typeparam name="T">The expression type.</typeparam>
        /// <param name="first">The base expression.</param>
        /// <param name="second">The expression to combine onto the base.</param>
        /// <returns>A new OR-combined predicate.</returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.IsStarted() ? first.Compose(second, Expression.OrElse) : second;
        }

        /// <summary>
        /// Negates the predicate.
        /// </summary>
        /// <typeparam name="T">The expression type.</typeparam>
        /// <param name="expression">The base expression.</param>
        /// <returns>A new negated expression.</returns>
        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expression)
        {
            var negated = Expression.Not(expression.Body);
            return Expression.Lambda<Func<T, bool>>(negated, expression.Parameters);
        }

        /// <summary>
        /// Determines if the predicate is started.
        /// </summary>
        /// <typeparam name="T">Type of expression.</typeparam>
        /// <param name="expression">Expression to check.</param>
        /// <returns>Returns true if predicate has been started (not simply constant true or false), false otherwise.</returns>
        public static bool IsStarted<T>(this Expression<Func<T, bool>> expression)
        {
            return !(expression.Body is ConstantExpression expr && expr.Type == typeof(bool));
        }

        /// <summary>
        /// Create a new predicate with default expression (false).
        /// </summary>
        /// <typeparam name="T">Type of expression.</typeparam>
        /// <returns>A new default expression.</returns>
        public static Expression<Func<T, bool>> New<T>()
        {
            return param => false;
        }

        /// <summary>
        /// Combines the first expression with the second using the specified merge function.
        /// </summary>
        private static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            // zip parameters (map from parameters of second to parameters of first)
            var map = first.Parameters
                .Select((f, i) => new { f, s = second.Parameters[i] })
                .ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with the parameters in the first
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            // create a merged lambda expression with parameters from the first expression
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        private class ParameterRebinder : ExpressionVisitor
        {
            private readonly Dictionary<ParameterExpression, ParameterExpression> map;

            private ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
            {
                this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
            }

            public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
            {
                return new ParameterRebinder(map).Visit(exp);
            }

            protected override Expression VisitParameter(ParameterExpression p)
            {
                if (this.map.TryGetValue(p, out ParameterExpression replacement))
                {
                    p = replacement;
                }

                return base.VisitParameter(p);
            }
        }
    }
}
