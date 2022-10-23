using System;

namespace DataAccess.Annotations
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    sealed public class FunctionAttribute : Attribute
    {
        public string? FunctionName { get; }
        public string? Schema { get; }

        public FunctionAttribute()
        {
        }

        public FunctionAttribute(string functionName, string? schema = null)
        {
            FunctionName = functionName;
            Schema = schema;
        }
    }
}
