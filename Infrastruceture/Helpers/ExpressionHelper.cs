using System.Linq.Expressions;

namespace Infrastructure.Helpers
{
    public static class ExpressionHelper
    {
        #region LINQ_EXPRESSION

        private static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            if (first == null && second == null)
                return null;

            if (first == null)
            {
                return second;
            }

            if (second == null)
            {
                return first;
            }

            // build parameter map (from parameters of second to parameters of first)
            var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with parameters from the first
            var secondBody = ParameterHelper.ReplaceParameters(map, second.Body);

            // apply composition of lambda expression bodies to parameters from the first expression 
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        public static Expression<Func<T, bool>> New<T>(Expression<Func<T, bool>> target)
        {
            return target;
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> target,
            params Expression<Func<T, bool>>[] expressions)
        {
            Expression<Func<T, bool>> resultExpression = target;

            if (target != null)
            {
                resultExpression = expressions.Aggregate(resultExpression, (current, t) => current.Compose(t, Expression.AndAlso));
            }

            return resultExpression;
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> target,
            params Expression<Func<T, bool>>[] expressions)
        {
            Expression<Func<T, bool>> resultExpression = target;

            if (target != null)
            {
                resultExpression = expressions.Aggregate(resultExpression, (current, t) => current.Compose(t, Expression.OrElse));
            }

            return resultExpression;
        }

        #endregion LINQ_EXPRESSION

        #region Utils

        public static string GetMemberName<T>(this Expression<T> expression)
        {
            switch (expression?.Body)
            {
                case MemberExpression m:
                    return m.Member.Name;

                default:
                    throw new ArgumentException(string.Format("The argument of type {0} is invalid.", expression?.GetType().ToString() ?? "null"));
            }
        }

        #endregion
    }

    public class ParameterHelper : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> map;

        public ParameterHelper(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
        {
            return new ParameterHelper(map).Visit(exp);
        }

        protected override Expression VisitParameter(ParameterExpression p)
        {
            ParameterExpression replacement;
            if (map.TryGetValue(p, out replacement))
            {
                p = replacement;
            }
            return base.VisitParameter(p);
        }
    }
}
