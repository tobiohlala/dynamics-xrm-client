using System;

namespace DynamicsXrmClient.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class LogicalNameAttribute : Attribute
    {
        /// <summary>
        /// The logical name of the entity represented by the class.
        /// </summary>
        public string LogicalName { get; }

        /// <summary>
        /// Initializes an <see cref="LogicalNameAttribute"/>.
        /// </summary>
        /// <param name="logicalName">
        /// The logical name of the entity represented by the class
        /// </param>
        public LogicalNameAttribute(string logicalName)
        {
            LogicalName = logicalName;
        }
    }
}
