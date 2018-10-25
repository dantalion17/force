using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Force.Ddd
{
    public class StringFieldExpressionBuilder : IFieldExpressionBuilder
    {
        private static MethodInfo StartsWith = typeof(string)
            .GetMethod("StartsWith", new[] {typeof(string)});


        public bool CanBuild(PropertyInfo property)
            => property.PropertyType.Equals(typeof(string));


        public Expression<Func<SubjectType, bool>> Build<SubjectType>(PropertyInfo propertyInfo, object Value)
        {
            var parameter = Expression.Parameter(typeof(SubjectType));

            Expression value = Expression.Constant(Value);

            var property = Expression.Property(parameter, propertyInfo);

            value = Expression.Convert(value, typeof(string));

            var body = Expression.Call(property, StartsWith, value);

            return Expression.Lambda<Func<SubjectType, bool>>(body, parameter);
        }
    }
}