using System;

namespace DataAccess.Annotations
{
    /// <summary>
    /// Apply this attribute to any class that maps to a view in the database
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    sealed public class ViewAttribute : Attribute
    {
        public string? ViewName { get; }
        public string? Schema { get; }

        /// <summary>
        /// Constructs an instance
        /// </summary>
        /// <param name="viewName">The name of the view in the database</param>
        /// <param name="schema">Optional schema of the view in the database</param>
        public ViewAttribute(string viewName, string? schema = null)
        {
            ViewName = viewName;
            Schema = schema;
        }
    }
}
