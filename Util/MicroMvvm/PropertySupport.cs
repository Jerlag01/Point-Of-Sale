using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Util.MicroMvvm
{
    public static class PropertySupport
    {
        public static string ExtractPropertyName<T>(Expression<Func<T>> propertyExpresssion)
        {
            if (propertyExpresssion == null)
                throw new ArgumentNullException(nameof(propertyExpresssion));
            if (!(propertyExpresssion.Body is MemberExpression body))
                throw new ArgumentException("The expression is not a member access expression.", nameof(propertyExpresssion));
            PropertyInfo member = body.Member as PropertyInfo;
            if (member == (PropertyInfo)null)
                throw new ArgumentException("The member access expression does not access a property.", nameof(propertyExpresssion));
            if (member.GetGetMethod(true).IsStatic)
                throw new ArgumentException("The referenced property is a static property.", nameof(propertyExpresssion));
            return body.Member.Name;
        }
    }
}