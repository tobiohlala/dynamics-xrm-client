using DynamicsXrmClient.Attributes;
using DynamicsXrmClient.Exceptions;
using System;
using System.Linq;

namespace DynamicsXrmClient.Extensions
{
    public static class AttributeExtensions
    {
        public static string GetLogicalName<T>(this T table)
        {
            return table.GetType().GetLogicalName();
        }

        public static string GetLogicalName(this Type table)
        {
            return table.GetAttributeValue((DataverseTableAttribute a) => a.LogicalName);
        }

        public static string GetLogicalCollectionName<T>(this T table)
        {
            return table.GetType().GetLogicalCollectionName();
        }

        public static string GetLogicalCollectionName(this Type table)
        {
            return table.GetAttributeValue((DataverseTableAttribute a) => a.LogicalCollectionName);
        }

        public static Guid GetDataverseRowId<T>(this T row)
        {
            var rowIdProperty = row
                .GetType()
                .GetProperties()
                .FirstOrDefault(p => Attribute.IsDefined(p, typeof(DataverseRowIdAttribute)));

            if (rowIdProperty != null)
            {
                var rowId = rowIdProperty.GetValue(row);

                if (rowId is Guid guid)
                {
                    return guid;
                }
            }

            throw new MissingAttributeException(row.GetType().ToString(), typeof(DataverseRowIdAttribute).ToString());
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
