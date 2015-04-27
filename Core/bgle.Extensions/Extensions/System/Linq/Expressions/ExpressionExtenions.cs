namespace System.Linq.Expressions
{
    public static class ExpressionExtenions
    {
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.And);
        }

        public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            // build parameter map (from parameters of second to parameters of first)
            var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with parameters from the first
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            // apply composition of lambda expression bodies to parameters from the first expression 
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        public static Expression<Func<T, bool>> Equals<T, TValue>(string propertyName, TValue constant)
        {
            var type = typeof(T);
            var parameterExpression = Expression.Parameter(type, type.Name);
            return Expression.Lambda<Func<T, bool>>(Expression.Equal(Expression.Property(parameterExpression, propertyName), Expression.Constant(constant)),
                new[] { parameterExpression });
        }

        public static string GetPropertyName<T>(Expression<Func<T>> propertyLambda)
        {
            var unaryExpression = propertyLambda.Body as UnaryExpression;
            var memberExpression = unaryExpression == null? propertyLambda.Body as MemberExpression : unaryExpression.Operand as MemberExpression;
            
            if (memberExpression == null)
            {
                throw new ArgumentException("You must pass a lambda of the form: '() => Class.Property' or '() => object.Property'");
            }

            return memberExpression.Member.Name;
        }

        public static string GetPropertyName<T, TProperty>(Expression<Func<T, TProperty>> propertyLambda)
        {
            var unaryExpression = propertyLambda.Body as UnaryExpression;
            var memberExpression = unaryExpression == null ? propertyLambda.Body as MemberExpression : unaryExpression.Operand as MemberExpression;

            if (memberExpression == null)
            {
                throw new ArgumentException("You must pass a lambda of the form: 'e => Class.Property' or 'e => e.Property'");
            }

            return memberExpression.Member.Name;
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.Or);
        }
    }
}