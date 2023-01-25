using System;

namespace BufTools.DataAnnotations.Schema
{
    /// <summary>
    /// Apply this attribute to any scalar or table function
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    sealed public class FunctionAttribute : Attribute
    {
        /// <summary>
        /// The name of the DB function a c# method si associated with
        /// </summary>
        public string FunctionName { get; }

        /// <summary>
        /// The DB schema of the table the function is associated with
        /// </summary>
        public string Schema { get; }

        /// <summary>
        /// Constructs an instance
        /// </summary>
        /// <param name="functionName">The name of the function in the database</param>
        /// <param name="schema">Option schema of the function in the database</param>
        public FunctionAttribute(string functionName, string schema = null)
        {
            FunctionName = functionName;
            Schema = schema;
        }
    }
}
