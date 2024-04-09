using System;

namespace BufTools.DataStore.Schema
{
    /// <summary>
    /// Apply this attribute to each param that together form a composite primary key
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    sealed public class CompositeKeyAttribute : Attribute
    {
    }
}
