using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using CareMobile.API.Common;
using CareMobile.Azure.DocumentDB;

namespace CareMobile.API.Host.Controllers
{
    public class LoginController : ApiController
    {
        private ILoginRepository _loginRepository;
        public LoginController(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }
        public async Task<ServiceResult<string>> Login(User user)
        {
            return await _loginRepository.Login(user);
        }
    }
}
