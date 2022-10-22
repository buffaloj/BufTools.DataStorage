using System;

namespace DataAccess.Annotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    sealed public class CompositeKeyAttribute : Attribute
    {
    }
}
