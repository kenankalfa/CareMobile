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
            //StreamTest().Wait();
            //JobApplicationDocumentDbCreateTest().Wait();
            //JobApplicationDocumentDbListTest().Wait();
            //PositionDocumentDbCreateTest().Wait();
            //Test().Wait();
            PositionDocumentDbListTest().Wait();

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
            var config = new ConfigurationSettings();
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://caremobileapihostv3.azurewebsites.net/");
            httpClient.BaseAddress = new Uri("http://localhost:15199/");

            var response = await httpClient.GetStringAsync("api/JobApplication/e749f671-22b6-4f83-b701-ee096ce8931c");
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
            //var config = new ConfigurationSettings();
            //var jobApplication = new JobApplicationRepository(config);

            //for (int i = 0; i < 5; i++)
            //{
            //    var instance = new JobApplication();

            //    instance.Applicant = new Applicant() { ApplicantRef = Guid.NewGuid().ToString(), CreateDate = DateTime.Now, FullName = Faker.NameFaker.FirstName(),IdentificationNumber = Faker.NumberFaker.Number().ToString() };
            //    instance.CreateDate = DateTime.Now;
            //    instance.ModifiedDate = Faker.DateTimeFaker.DateTime();
            //    instance.Photo = new Photo() { FileName = "ronaldo.jpg", PhotoRef = Guid.NewGuid().ToString(), Url = "http://assets.hookit.com/RFS/2017/02/16/a4469d70-0cad-4030-82d3-5530e10bfea4.jpg?size=200&crop=true" };
            //    instance.Position = new Position() { PositionName = Faker.StringFaker.Alpha(30), PositionRef = Guid.NewGuid().ToString() };

            //    await jobApplication.Save(instance);
            //}
        }

        static async Task JobApplicationDocumentDbListTest()
        {
            //var config = new ConfigurationSettings();
            //var jobApplication = new JobApplicationRepository(config);

            //var getResult = await jobApplication.Get(q => q.IsApprovedForHarmony == true);
            //System.Console.WriteLine("liste");
            //foreach (var item in getResult)
            //{
            //    System.Console.WriteLine(item.JobApplicationRef + " " + item.Applicant.FullName);
            //}
        }

        static async Task EmotionCreate()
        {
            var config = new ConfigurationSettings();
            //var jobApplication = new EmotionApiRepository(config);

            //for (int i = 0; i < 5; i++)
            //{
            //    var instance = new EmotionApiResult();

            //    instance.JobApplicantRef = Guid.NewGuid().ToString();
            //    instance.EmotionApi = new RootObject();

            //    await jobApplication.Save(instance);
            //}
        }

        static async Task PositionDocumentDbCreateTest()
        {
            //var config = new ConfigurationSettings();
            //var jobApplication = new PositionRepository(config);

            //for (int i = 0; i < 5; i++)
            //{
            //    var instance = new Position();

            //    instance.IsDeleted = false;
            //    instance.PositionName = "Pozisyon " + (i+1);
            //    instance.PositionRef = Guid.NewGuid().ToString();
                
            //    await jobApplication.Save(instance);
            //}
        }

        static async Task PositionDocumentDbListTest()
        {
            //var config = new ConfigurationSettings();
            //var position = new PositionRepository(config);

            //var getResult = await position.Get(q => q.IsDeleted == false && q.PositionRef == "0ac35922-1939-4672-bc65-85ee084c70f0");
            //System.Console.WriteLine("liste");
            //foreach (var item in getResult)
            //{
            //    System.Console.WriteLine(item.PositionRef + " " + item.PositionName);
            //}
        }
    }
}
