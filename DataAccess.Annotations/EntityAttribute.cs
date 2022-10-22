using System;

namespace DataAccess.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    sealed public class EntityAttribute : Attribute
    {
        public string? TableName { get; }
        public string? Schema { get; }

        public EntityAttribute()
        {
        }

        public EntityAttribute(string tableName, string? schema = null)
        {
            TableName = tableName;
            Schema = schema;
        }
    }
}
