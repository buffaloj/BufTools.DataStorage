using System;

namespace DataAccess.Annotations
{
    /// <summary>
    /// Apply this attribute to each param that together form a promary key
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    sealed public class CompositeKeyAttribute : Attribute
    {
    }
}
