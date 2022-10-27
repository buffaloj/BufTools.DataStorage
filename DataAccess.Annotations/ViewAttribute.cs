using System;

namespace DataAccess.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    sealed public class ViewAttribute : Attribute
    {
        public string? ViewName { get; }
        public string? Schema { get; }

        public ViewAttribute()
        {
        }

        public ViewAttribute(string tableName, string? schema = null)
        {
            ViewName = tableName;
            Schema = schema;
        }
    }
}
