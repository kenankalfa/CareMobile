using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CareMobile.Azure.DocumentDB
{
    using CareMobile.API.Common;
    public interface IPositionRepository
    {
        Task<IEnumerable<Position>> Get(Expression<Func<Position,bool>> predicate);
        Task Save(Position instance);
    }
}
