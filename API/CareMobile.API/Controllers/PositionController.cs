using Microsoft.Azure.Mobile.Server.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using CareMobile.Azure.DocumentDB;
using CareMobile.Azure.Storage;
using CareMobile.API.Common;
using System.Threading.Tasks;

namespace CareMobile.API.Controllers
{
    [MobileAppController]
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