using System;

namespace DynamicsXrmClient.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class LogicalCollectionNameAttribute : Attribute
    {
        /// <summary>
        /// The logical collection name of the entity represented by the class.
        /// </summary>
        public string LogicalCollectionName { get; }

        /// <summary>
        /// Initializes an <see cref="LogicalCollectionNameAttribute"/>.
        /// </summary>
        /// <param name="entityLogicalCollectionName">
        /// The logical collection name of the entity represented by the class
        /// </param>
        public LogicalCollectionNameAttribute(string entityLogicalCollectionName)
        {
            LogicalCollectionName = entityLogicalCollectionName;
        }
    }
}
