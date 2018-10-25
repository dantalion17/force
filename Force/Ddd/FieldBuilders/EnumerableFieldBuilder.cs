using System;
using System.Collections;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;

namespace Force.Ddd
{
    public class EnumerableFieldBuilder : IFieldExpressionBuilder
    {
        private static MethodInfo Contains = typeof(Enumerable)
            .GetMethods()
            .First(x => x.Name == "Contains" && x.GetParameters().Length == 2);

        public bool CanBuild(PropertyInfo prop)
        {
            var s =  typeof(IEnumerable).IsAssignableFrom(prop.PropertyType);
            return typeof(IEnumerable).IsAssignableFrom(prop.PropertyType);
        }

        public Expression<Func<SubjectType, bool>> Build<SubjectType>(PropertyInfo propertyInfo, object Value)
        {
            var objectType = typeof(object);
            var parameter = Expression.Parameter(typeof(SubjectType));
            var property = Expression.Property(parameter, propertyInfo);
            Expression body;
            Expression value;

            if (Value is IEnumerable enumerable)
            {
                var array = enumerable
                    .ToDynamicArray()
                    .Select(v => v.Equals(0) || v.Equals("null") ? null : v)
                    .Cast<object>()
                    .ToArray();

                value = Expression.Constant(array);
            }
            else
                throw new Exception("Type must implement IEnumerable");

            var converted = Expression.Convert(property, objectType);
            body = Expression.Call(null, Contains.MakeGenericMethod(objectType), value, converted);
            return Expression.Lambda<Func<SubjectType, bool>>(body, parameter);
        }
    }
}