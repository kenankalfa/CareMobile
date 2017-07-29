using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CareMobileApp.Utils
{
    public static class NavigationMessage
    {
        private static object lockObject = new object();
        public static void PutData<T>(string key, T value)
        {
            lock (lockObject)
            {
                Application.Current.Properties[key] = value;
            }
        }

        public static T GetData<T>(string key)
        {
            lock (lockObject)
            {
                T value = default(T);

                IDictionary<string, object> iDictionary = Application.Current.Properties;

                if (iDictionary.ContainsKey(key))
                {
                    value = (T)iDictionary[key];
                }

                return value;
            }
        }
    }

    public static class Const
    {
        public const string JobApplicationPagesData = "JobApplicationPagesData";
        public const string APIUrl = "http://caremobileapihostv3.azurewebsites.net";
    }

    public class JobApplicationPagesData
    {
        public MemoryStream PhotoStream { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime BirthDate { get; set; }
        public string SelectedPosition { get; set; }

        public bool IsValid
        {
            get
            {
                var returnValue = false;

                if (
                    PhotoStream != null
                    ||
                    !String.IsNullOrEmpty(FullName)
                    ||
                    !String.IsNullOrEmpty(EmailAddress)
                    ||
                    BirthDate != DateTime.Now
                    ||
                    !String.IsNullOrEmpty(SelectedPosition)
                    )
                {
                    returnValue = true;
                }

                return returnValue;
            }
        }
    }

    public static class JobApplicationPagesDataManager
    {
        public static JobApplicationPagesData Instance
        {
            get
            {

                var instance = NavigationMessage.GetData<JobApplicationPagesData>(Const.JobApplicationPagesData);
                if (instance == null)
                {
                    instance = new JobApplicationPagesData();
                    NavigationMessage.PutData<JobApplicationPagesData>(Const.JobApplicationPagesData, instance);
                }
                return instance;
            }
        }
        public static void Clear()
        {
            NavigationMessage.PutData<JobApplicationPagesData>(Const.JobApplicationPagesData, null);
        }
        public static void SetPhotoStream(MemoryStream ms)
        {
            Instance.PhotoStream = ms;
            NavigationMessage.PutData<JobApplicationPagesData>(Const.JobApplicationPagesData, Instance);
        }
        public static void SetFormData(JobApplicationPagesData formData)
        {
            Instance.BirthDate = formData.BirthDate;
            Instance.EmailAddress = formData.EmailAddress;
            Instance.FullName = formData.FullName;
            Instance.SelectedPosition = formData.SelectedPosition;

            NavigationMessage.PutData<JobApplicationPagesData>(Const.JobApplicationPagesData, Instance);
        }
    }

    public static class HttpServices
    {
        public static class PositionService
        {
            public static async Task<IEnumerable<Position>> GetPositions()
            {
                var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(Const.APIUrl);

                var getResult = await httpClient.GetAsync("api/Position");
                var getResultContent = await getResult.Content.ReadAsStringAsync();
                var resultObject = JsonConvert.DeserializeObject<ServiceResult<IEnumerable<Position>>>(getResultContent);

                return resultObject.Result;
            }
        }
        public static class JobApplicationService
        {
            public static async Task<IEnumerable<JobApplication>> GetApproveList()
            {
                var httpClient = new HttpClient();

                httpClient.BaseAddress = new Uri(Const.APIUrl);
                httpClient.Timeout = TimeSpan.FromMinutes(3);

                var getResult = await httpClient.GetAsync("api/JobApplication?isApprovedForHarmony=");
                var getResultContent = await getResult.Content.ReadAsStringAsync();
                var resultObject = JsonConvert.DeserializeObject<ServiceResult<IEnumerable<JobApplication>>>(getResultContent);

                return resultObject.Result;
            }
            public static async Task<ServiceResult<bool>> MakeJobApplication(MemoryStream profilePhotoStreamOnDisappearing)
            {
                var returnValue = default(Task<ServiceResult<bool>>);

                try
                {
                    var currentInstance = JobApplicationPagesDataManager.Instance;

                    var postPhotoStream = new MemoryStream(profilePhotoStreamOnDisappearing.ToArray());
                    postPhotoStream.Position = 0;

                    var httpClient = new HttpClient();
                    httpClient.Timeout = TimeSpan.FromMinutes(1);
                    httpClient.BaseAddress = new Uri(Const.APIUrl);

                    var instance = new JobApplication();

                    instance.Applicant = new Applicant();
                    instance.Applicant.BirthDate = currentInstance.BirthDate;
                    instance.Applicant.EmailAddress = currentInstance.EmailAddress;
                    instance.Applicant.FullName = currentInstance.FullName;
                    instance.Position = new Position();
                    instance.Position.PositionName = currentInstance.SelectedPosition;

                    var request = new HttpRequestMessage();
                    var requestContent = new MultipartFormDataContent();
                    var streamContent = new StreamContent(postPhotoStream);
                    var stringContent = new StringContent(JsonConvert.SerializeObject(instance), Encoding.UTF8, "application/json");

                    request.Method = HttpMethod.Post;

                    requestContent.Add(streamContent, "PhotoStreamInstance");
                    requestContent.Add(stringContent, "EntityInstance");

                    request.Content = requestContent;

                    var response = await httpClient.PostAsync("api/JobApplication/Post", requestContent);
                    var contentString = await response.Content.ReadAsStringAsync();

                    var resultObject = JsonConvert.DeserializeObject<ServiceResult<bool>>(contentString);
                    returnValue = Task.FromResult<ServiceResult<bool>>(resultObject);
                }
                catch (Exception ex)
                {
                    var exception = new ServiceResult<bool>();
                    exception.Messages = new string[] { ex.Message };

                    if (returnValue == default(Task<ServiceResult<bool>>))
                    {
                        returnValue = Task.FromResult<ServiceResult<bool>>(exception);
                    }
                }

                return await returnValue;
            }
        }
    }

    public static class UITaskFactory
    {
        public async static Task<int> StartNew(Action action)
        {
            var cancellationTokenSource = new CancellationTokenSource();

            await Task.Factory.StartNew(
                (object state)
                =>
                    {
                        action();
                    },
                    TaskCreationOptions.LongRunning,
                    cancellationTokenSource.Token);

            return 1;
        }
    }
}
