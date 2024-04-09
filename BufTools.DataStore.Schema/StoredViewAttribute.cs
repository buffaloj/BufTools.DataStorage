using System;

namespace BufTools.DataStore.Schema
{
    /// <summary>
    /// Apply this attribute to any class that maps to a view in the database
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    sealed public class StoredViewAttribute : Attribute
    {
        /// <summary>
        /// The name of the view the View class is associated with
        /// </summary>
        public string ViewName { get; }

        /// <summary>
        /// The DB schema of the table the Entity is associated with
        /// </summary>
        public string Schema { get; }

        /// <summary>
        /// Constructs an instance
        /// </summary>
        /// <param name="viewName">The name of the view in the database</param>
        /// <param name="schema">Optional schema of the view in the database</param>
        //public ViewAttribute(string viewName, string schema = null)
        //{
        //    ViewName = viewName;
        //    Schema = schema;
        //}
    }
}
