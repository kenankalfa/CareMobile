using CareMobile.API.Common;
using CareMobile.Azure.DocumentDB;
using System;
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
        public async Task<ServiceResult<IEnumerable<Position>>> Get()
        {
            var returnValue = new ServiceResult<IEnumerable<Position>>();
            returnValue.Result = await _positionRepository.Get(q => !q.IsDeleted);
            returnValue.IsSucceed = true;
            return returnValue;
        }
        public async Task<ServiceResult<Position>> Get(string id)
        {
            var returnValue = new ServiceResult<Position>();

            var result = await _positionRepository.Get(q => !q.IsDeleted && q.PositionRef == id);
            returnValue.IsSucceed = result != null && result.Any();

            if (returnValue.IsSucceed)
            {
                returnValue.Result = result.FirstOrDefault();
            }
            
            return returnValue;
        }
    }
}