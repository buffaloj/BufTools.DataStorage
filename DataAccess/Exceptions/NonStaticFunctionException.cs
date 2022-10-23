using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DataAccess.Exceptions
{
    /// <summary>
    /// Thrown when a DataAccess Function is not static
    /// </summary>
    [Serializable]
    public class NonStaticFunctionException : Exception
    {
        /// <summary>
        /// A collection of method names that are not static, but should be
        /// </summary>
        public IEnumerable<string>? InvalidMethods { get; }

        /// <summary>
        /// Constructs an instance
        /// </summary>
        /// <param name="invalidMethods">The name of the methods are supposed to be static</param>
        public NonStaticFunctionException(IEnumerable<string> invalidMethods)
            : base($"Functions must be static: {string.Join(", ", invalidMethods)}")
        {
            InvalidMethods = invalidMethods;
        }

        protected NonStaticFunctionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
