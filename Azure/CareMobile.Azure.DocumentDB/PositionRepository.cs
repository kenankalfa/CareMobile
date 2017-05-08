using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Linq.Expressions;
using CareMobile.API.Configuration;
using CareMobile.API.Common;

namespace CareMobile.Azure.DocumentDB
{
    public class PositionRepository : DocumentDBRepository<Position>, IPositionRepository
    {
        public PositionRepository(IConfigurationSettings settings):base(settings)
        {

        }

        public Task<IEnumerable<Position>> Get(Expression<Func<Position, bool>> predicate)
        {
            return GetItemsAsync(predicate);
        }
    }
}
