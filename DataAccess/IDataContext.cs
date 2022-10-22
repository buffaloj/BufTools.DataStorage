using System;
using System.Collections.Generic;

namespace DataAccess
{
    public interface IDataContext
    {
        IEnumerable<Type> GetTypesToRegister();
    }
}
