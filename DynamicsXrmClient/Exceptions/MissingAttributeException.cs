using System;

namespace DynamicsXrmClient.Exceptions
{
    public class MissingAttributeException : Exception
    {
        public MissingAttributeException(string type, string attribute) :
            base($"{type} is missing expected attribute {attribute}")
        {
        }
    }
}
