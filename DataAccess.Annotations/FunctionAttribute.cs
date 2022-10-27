using System;

namespace DataAccess.Annotations
{
    /// <summary>
    /// Apply this attribute to any scalar or table function
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    sealed public class FunctionAttribute : Attribute
    {
        public string? FunctionName { get; }
        public string? Schema { get; }

        /// <summary>
        /// Constructs an instance
        /// </summary>
        /// <param name="functionName">The name of the function in the database</param>
        /// <param name="schema">Option schema of the function in the database</param>
        public FunctionAttribute(string functionName, string? schema = null)
        {
            FunctionName = functionName;
            Schema = schema;
        }
    }
}
