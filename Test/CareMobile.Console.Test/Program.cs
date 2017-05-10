using CareMobile.API.Configuration;
using CareMobile.Azure.DocumentDB;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using CareMobile.API.Common;
using System.IO;
using Newtonsoft.Json;

namespace CareMobile.Console.Test
{
    class Program
    {
        // for testing purposes
        static void Main(string[] args)
        {
            StreamTest().Wait();
            // JobApplicationDocumentDbCreateTest().Wait();
            // JobApplicationDocumentDbListTest().Wait();

            // EmotionCreate().Wait();

            System.Console.WriteLine("bitti");
            System.Console.ReadLine();

        }

        static async Task StreamTest()
        {
            var postPhotoStream = new FileStream(@"localfilepath", FileMode.Open);
            
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://yourazure.web.api");

            var instance = new JobApplication();

            instance.Applicant = new Applicant();
            instance.Applicant.BirthDate = DateTime.Now.AddYears(-10);
            instance.Applicant.EmailAddress = "kenan.kalfa@kenankalfa.com";
            instance.Applicant.FullName = "kenan kalfa " + DateTime.Now.ToString();
            instance.Position = new Position();
            instance.Position.PositionName = "Pozisyon 2";

            var request = new HttpRequestMessage();
            var requestContent = new MultipartFormDataContent();
            var streamContent = new StreamContent(postPhotoStream);
            var stringContent = new StringContent(JsonConvert.SerializeObject(instance), Encoding.UTF8, "application/json");

            request.Method = HttpMethod.Post;

            requestContent.Add(streamContent, "PhotoStreamInstance");
            requestContent.Add(stringContent, "EntityInstance");

            request.Content = requestContent;

            System.Console.WriteLine("start");
            System.Console.ReadLine();
            var response = await httpClient.PostAsync("api/JobApplication/Post", requestContent);
            var str = await response.Content.ReadAsStringAsync();

            System.Console.WriteLine(str);
            System.Console.ReadLine();
        }

        static async Task Test()
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://caremobileaphostv2.azurewebsites.net/");
            
            var response = await httpClient.GetStringAsync("api/JobApplication/56c383b2-ac63-4464-baf1-7e826fd47668");
            System.Console.WriteLine(response);
        }

        public class ConfigurationSettings : IConfigurationSettings
        {
            public ConfigurationSettings()
            {
                InitializeSettings();
            }

            public string AzureDocumentDbDatabaseId { get; set; }
            public string AzureDocumentDbEndPoint { get; set; }
            public string AzureDocumentDbAuthKey { get; set; }
            public string AzureStorageEndPoint { get; set; }
            public string AzureStorageContainerName { get; set; }
            public string AzureStorageContainerDirectoryName { get; set; }
            public string AzureEmotionApiKey { get; set; }
            public string AzureEmotionApiEndPoint { get; set; }

            public void InitializeSettings()
            {
                AzureDocumentDbDatabaseId = ConfigurationManager.AppSettings["AzureDocumentDbDatabaseId"];
                AzureDocumentDbEndPoint = ConfigurationManager.AppSettings["AzureDocumentDbEndPoint"];
                AzureDocumentDbAuthKey = ConfigurationManager.AppSettings["AzureDocumentDbAuthKey"];
                AzureStorageEndPoint = ConfigurationManager.AppSettings["AzureStorageEndPoint"];
                AzureStorageContainerName = ConfigurationManager.AppSettings["AzureStorageContainerName"];
                AzureStorageContainerDirectoryName = ConfigurationManager.AppSettings["AzureStorageContainerDirectoryName"];
                AzureEmotionApiKey = ConfigurationManager.AppSettings["AzureEmotionApiKey"];
                AzureEmotionApiEndPoint = ConfigurationManager.AppSettings["AzureEmotionApiEndPoint"];
            }
        }

        static async Task JobApplicationDocumentDbCreateTest()
        {
            var config = new ConfigurationSettings();
            var jobApplication = new JobApplicationRepository(config);

            for (int i = 0; i < 5; i++)
            {
                var instance = new JobApplication();

                instance.Applicant = new Applicant() { ApplicantRef = Guid.NewGuid().ToString(), CreateDate = DateTime.Now, FullName = Faker.NameFaker.FirstName(),IdentificationNumber = Faker.NumberFaker.Number().ToString() };
                instance.CreateDate = DateTime.Now;
                instance.ModifiedDate = Faker.DateTimeFaker.DateTime();
                instance.Photo = new Photo() { FileName = "maxresdefault.jpg", PhotoRef = Guid.NewGuid().ToString(), Url = "https://i.ytimg.com/vi/ucZZWg3LECg/maxresdefault.jpg" };
                instance.Position = new Position() { PositionName = Faker.StringFaker.Alpha(30), PositionRef = Guid.NewGuid().ToString() };

                await jobApplication.Save(instance);
            }
        }

        static async Task JobApplicationDocumentDbListTest()
        {
            var config = new ConfigurationSettings();
            var jobApplication = new JobApplicationRepository(config);

            var getResult = await jobApplication.Get(q => q.IsApprovedForHarmony == null);
            System.Console.WriteLine("liste");
            foreach (var item in getResult)
            {
                System.Console.WriteLine(item.JobApplicationRef + " " + item.Applicant.FullName);
            }
        }

        static async Task EmotionCreate()
        {
            var config = new ConfigurationSettings();
            var jobApplication = new EmotionApiRepository(config);

            for (int i = 0; i < 5; i++)
            {
                var instance = new EmotionApiResult();

                instance.JobApplicantRef = Guid.NewGuid().ToString();
                instance.EmotionApi = new RootObject();

                await jobApplication.Save(instance);
            }
        }
    }
}
