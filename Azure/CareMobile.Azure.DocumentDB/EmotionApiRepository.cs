using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CareMobile.API.Common;
using CareMobile.API.Configuration;

namespace CareMobile.Azure.DocumentDB
{
    public class EmotionApiRepository : DocumentDBRepository<EmotionApiResult>, IEmotionApiRepository
    {
        private IConfigurationSettings _settings;
        public EmotionApiRepository(IConfigurationSettings settings):base(settings)
        {
            
        }
        public async Task Save(EmotionApiResult instance)
        {
            await CreateItemAsync(instance);
        }
    }
}
