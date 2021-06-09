using System;

namespace DynamicsXrmClient.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DataverseTableAttribute : Attribute
    {
        /// <summary>
        /// The logical name of the table.
        /// </summary>
        public string LogicalName { get; }

        /// <summary>
        /// The logical collection name of the table.
        /// </summary>
        public string LogicalCollectionName { get; }

        /// <summary>
        /// <see cref="DataverseTableAttribute"/>.
        /// </summary>
        /// <param name="logicalName">
        /// The logical name of the table.
        /// </param>
        /// <param name="logicalCollectionName">
        /// The logical collection name of the table.
        /// </param>
        public DataverseTableAttribute(string logicalName, string logicalCollectionName)
        {
            LogicalName = logicalName;
            LogicalCollectionName = logicalCollectionName;
        }
    }
}
