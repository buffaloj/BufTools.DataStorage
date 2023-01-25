using System;

namespace BufTools.EntityFrameworkCore.DataAutoWiring.Annotations
{
    /// <summary>
    /// Apply this attribute to any class that maps to a table or return value of a function or sproc
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    sealed public class EntityAttribute : Attribute
    {
        public string TableName { get; }
        public string Schema { get; }

        /// <summary>
        /// Constructs an instance that is NOT associated to a specific table.  ie, is a return value for a table func ot sproc
        /// </summary>
        public EntityAttribute()
        {
        }

        /// <summary>
        /// Constructs an instance that is associated to a specific table.
        /// </summary>
        /// <param name="tableName">The name of the table the entity is associated with</param>
        /// <param name="schema">An optional param for the schema of the table</param>
        public EntityAttribute(string tableName, string schema = null)
        {
            TableName = tableName;
            Schema = schema;
        }
    }
}
