using System;
using System.Linq.Expressions;

namespace Configgy
{
    public static class ConfigProviderExtensions
    {
        /// <summary>
        /// Adds an attribute for a given property.
        /// </summary>
        /// <param name="provider">The provider to add the attributes to.</param>
        /// <param name="property">A lambda indicating the property to add the attribute to.</param>
        /// <param name="attributes">The attribute to add.</param>
        public static void AddAttributes<TConfig, TProperty>(this IConfigProvider provider, Expression<Func<TConfig, TProperty>> property, params object[] attributes)
        {
            if (!(property.Body is MemberExpression propertyExpression))
            {
                throw new ArgumentException("Expression must be a simple property access lambda such as 'x => x.Property'", nameof(property));
            }

            foreach(var attribute in attributes)
            {
                provider.AddAttribute(propertyExpression.Member.Name, attribute);
            }
        }
    }
}