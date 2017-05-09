using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CareMobile.API.Common;

namespace CareMobile.Azure.DocumentDB
{
    public interface IEmotionApiRepository
    {
        Task Save(EmotionApiResult instance);
    }
}
