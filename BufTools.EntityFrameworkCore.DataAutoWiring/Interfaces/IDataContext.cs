using System;
using System.Collections.Generic;

namespace BufTools.EntityFrameworkCore.DataAutoWiring.Interfaces
{
    /// <summary>
    /// An interface to supply the types that might represent an entity, view, or function in the database.
    /// Think of it as dependency injection registration for the database
    /// </summary>
    public interface IDataContext
    {
        /// <summary>
        /// Returns a list of all class types that might be related to the database
        /// </summary>
        /// <returns>A collection of <see cref="Type"/></returns>
        /// <remarks>Mark the classes with [Entity(...)], [View(...)], or [Function(...)]</remarks>
        IEnumerable<Type> GetTypesToRegister();
    }
}
