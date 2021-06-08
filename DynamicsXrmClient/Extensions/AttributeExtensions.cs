using DynamicsXrmClient.Attributes;
using DynamicsXrmClient.Exceptions;
using System;
using System.Linq;

namespace DynamicsXrmClient.Extensions
{
    public static class AttributeExtensions
    {
        public static string GetLogicalCollectionName(this Type entity)
        {
            return entity.GetAttributeValue((LogicalCollectionNameAttribute a) => a.LogicalCollectionName);
        }

        public static string GetLogicalName(this Type entity)
        {
            return entity.GetAttributeValue((LogicalNameAttribute a) => a.LogicalName);
        }

        private static T GetAttributeValue<A, T>(this Type type, Func<A, T> valueSelector) where A : Attribute
        {
            if (type.GetCustomAttributes(typeof(A), true).FirstOrDefault() is A attribute)
            {
                return valueSelector(attribute);
            }

            throw new MissingAttributeException(type.ToString(), typeof(A).ToString());
        }
    }
}
