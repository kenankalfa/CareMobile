using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CareMobile.API.Configuration;
using System.Net.Http;
using CareMobile.Azure.Storage;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using CareMobile.API.Common;

namespace CareMobile.Azure.EmotionAPI
{
    public class EmotionRepository : IEmotionRepository
    {
        private IConfigurationSettings _settings;
        private IStorageRepository _storage;
        
        public EmotionRepository(IConfigurationSettings settings,IStorageRepository storage)
        {
            _settings = settings;
            _storage = storage;
        }

        public async Task<RootObject> GetEmotion(string fileName)
        {
            var returnValue = new RootObject();
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _settings.AzureEmotionApiKey);

            HttpResponseMessage response;
            string responseContent;
            
            byte[] byteData = _storage.GetByteArray(fileName);

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(_settings.AzureEmotionApiEndPoint, content);
                responseContent = response.Content.ReadAsStringAsync().Result;

                if (!String.IsNullOrEmpty(responseContent))
                {
                    returnValue = JsonConvert.DeserializeObject<RootObject>(responseContent);
                }
            }

            return returnValue;
        }
    }
}
