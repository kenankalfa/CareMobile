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
    public class PositionRepository : IPositionRepository
    {
        private IDocumentDBRepository<Position> _repository;

        public PositionRepository(IDocumentDBRepository<Position> repository, IConfigurationSettings settings)
        {
            _repository = repository;
        }

        public Task<IEnumerable<Position>> Get(Expression<Func<Position, bool>> predicate)
        {
            return _repository.GetItemsAsync(predicate);
        }

        public async Task Save(Position instance)
        {
            if (instance == null)
            {
                return;
            }

            await _repository.CreateItemAsync(instance);
        }
    }
}
