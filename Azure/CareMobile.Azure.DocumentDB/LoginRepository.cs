using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CareMobile.API.Common;
using CareMobile.API.Configuration;

namespace CareMobile.Azure.DocumentDB
{
    public class LoginRepository : DocumentDBRepository<User>, ILoginRepository
    {
        private ITokenRepository _tokenRepository;
        public LoginRepository(IConfigurationSettings settings,ITokenRepository tokenRepository):base(settings)
        {
            _tokenRepository = tokenRepository;
        }

        public async Task<bool> Create(User user)
        {
            var response = await CreateItemAsync(user);
            return response != null && !String.IsNullOrEmpty(response.Id) ? true : false;
        }

        public async Task<ServiceResult<string>> Login(User user)
        {
            var token = string.Empty;

            var returnValue = new ServiceResult<string>();

            var response = await GetItemsAsync(q => q.UserName == user.UserName && q.UserPassword == user.UserPassword);

            if (response != null && response.Any())
            {
                token = Guid.NewGuid().ToString();
                returnValue.Result = token;
                returnValue.IsSucceed = true;
                await _tokenRepository.StoreToken(user.UserName, token);
            }
            else
            {
                returnValue.Messages.Add("login failed.");
            }
            
            return returnValue;
        }
    }
}
