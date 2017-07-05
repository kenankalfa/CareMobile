using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CareMobile.API.Common;
using CareMobile.API.Configuration;

namespace CareMobile.Azure.DocumentDB
{
    public class EmotionApiRepository : IEmotionApiRepository
    {
        private IDocumentDBRepository<EmotionApiResult> _repository;

        public EmotionApiRepository(IDocumentDBRepository<EmotionApiResult> repository,IConfigurationSettings settings)
        {
            _repository = repository;
        }
        public async Task Save(EmotionApiResult instance)
        {
            await _repository.CreateItemAsync(instance);
        }
    }
}
