using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace bgle.Graph.Rexpro
{
    public static class DictionaryExtensions
    {
        public static void AddPascalCase<TValue>(this Dictionary<string, TValue> dic, Expression<Func<TValue>> propertyLambda)
        {

            dic.Add(ExpressionExtenions.GetPropertyName(propertyLambda).FirstCharacterToLower(), propertyLambda.Compile().Invoke());
        }
    }
}