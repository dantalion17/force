using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace Force.Ddd
{
    public interface IFieldExpressionBuilder
    {
        bool CanBuild(PropertyInfo prop);
        Expression<Func<SubjectType, bool>> Build<SubjectType>(PropertyInfo propertyInfo, object Value);
    }

    public class ValueTypeFieldBuilder : IFieldExpressionBuilder
    {
        public bool CanBuild(PropertyInfo prop)
        {
            var sss = prop.PropertyType.IsValueType;
            return prop.PropertyType.IsValueType;
        }

        public Expression<Func<SubjectType, bool>> Build<SubjectType>(PropertyInfo propertyInfo, object Value)
        {
            var parameter = Expression.Parameter(typeof(SubjectType));
            Expression value = Expression.Constant(Value);
            var property = Expression.Property(parameter, propertyInfo);
            value = Expression.Convert(value, propertyInfo.PropertyType);
            var body = Expression.Equal(property, value);

            return Expression.Lambda<Func<SubjectType, bool>>(body, parameter);
        }
    }
}