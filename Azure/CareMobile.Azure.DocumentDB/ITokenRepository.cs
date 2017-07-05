using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareMobile.Azure.DocumentDB
{
    public interface ITokenRepository
    {
        Task<bool> Check(string userName, string token);
        Task StoreToken(string userName,string token);
    }
}
