using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CareMobile.API.Common;

namespace CareMobile.Azure.EmotionAPI
{
    public interface IEmotionRepository
    {
        Task<RootObject> GetEmotion(string fileName);
    }
}
