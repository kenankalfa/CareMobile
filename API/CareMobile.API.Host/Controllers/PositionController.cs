using CareMobile.API.Common;
using CareMobile.Azure.DocumentDB;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace CareMobile.API.Host.Controllers
{
    public class PositionController : ApiController
    {
        private IPositionRepository _positionRepository;

        public PositionController(IPositionRepository positionRepository)
        {
            _positionRepository = positionRepository;
        }

        public async Task<IEnumerable<Position>> Get()
        {
            return await _positionRepository.Get(q => !q.IsDeleted);
        }
    }
}