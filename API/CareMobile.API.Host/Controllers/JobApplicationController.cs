using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using CareMobile.Azure.DocumentDB;
using CareMobile.Azure.Storage;
using CareMobile.API.Common;
using CareMobile.Azure.EmotionAPI;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Web;

namespace CareMobile.API.Host.Controllers
{
    public class JobApplicationController : ApiController
    {
        private IJobApplicationRepository _jobApplicationRepository;
        private IStorageRepository _storageRepository;
        private IEmotionRepository _emotionRepository;
        private IEmotionApiRepository _emotionApiRepository;
        
        public JobApplicationController(IJobApplicationRepository jobApplicationRepository,IStorageRepository storageRepository, IEmotionRepository emotionRepository,IEmotionApiRepository emotionApiRepository)
        {
            _jobApplicationRepository = jobApplicationRepository;
            _storageRepository = storageRepository;
            _emotionRepository = emotionRepository;
            _emotionApiRepository = emotionApiRepository;
        }
        public async Task<ServiceResult<IEnumerable<JobApplication>>> Get(bool? isApprovedForHarmony)
        {
            var returnValue = new ServiceResult<IEnumerable<JobApplication>>();
            returnValue.IsSucceed = true;
            returnValue.Result = await _jobApplicationRepository.Get(q => q.IsApprovedForHarmony == isApprovedForHarmony);
            return returnValue;
        }
        
        public async Task<ServiceResult<JobApplication>> Get(string id)
        {
            var returnValue = new ServiceResult<JobApplication>();
            returnValue.IsSucceed = true;
            var result = await _jobApplicationRepository.Get(q => q.JobApplicationRef == id);
            returnValue.Result = result.FirstOrDefault();
            return returnValue;
        }

        public async Task<ServiceResult<bool>> Post()
        {
            var returnValue = new ServiceResult<bool>();

            try
            {
                if (Request.Content == null)
                {
                    throw new Exception("Request.Content is null");
                }

                string root = HttpContext.Current.Server.MapPath("~/App_Data");
                var provider = new MultipartFormDataStreamProvider(root);
                var mpfdc = new MultipartFormDataContent();

                var mprovider = await Request.Content.ReadAsMultipartAsync(provider);
                
                var photoStream = mprovider.Contents.FirstOrDefault() as StreamContent;
                var stream = await photoStream.ReadAsStreamAsync();
                var entityAsString = mprovider.Contents.LastOrDefault() as StreamContent;

                var str = await entityAsString.ReadAsStringAsync();
                
                var postedJobApplication = JsonConvert.DeserializeObject<JobApplication>(str);

                var photoFileName = Guid.NewGuid().ToString() + ".jpg";
                var photoUrl = _storageRepository.UploadFile(photoFileName, stream);

                var emotionResult = await _emotionRepository.GetEmotion(photoFileName);

                if (emotionResult.scores.happiness <= 0.8)
                {
                    throw new Exception("You're not suitable for this position.");
                }

                var jobApplicant = new JobApplication();

                jobApplicant.CreateDate = DateTime.Now;
                jobApplicant.ModifiedDate = DateTime.Now;
                jobApplicant.Applicant = new Applicant();
                jobApplicant.Applicant.ApplicantRef = Guid.NewGuid().ToString();
                jobApplicant.Applicant.CreateDate = DateTime.Now;
                jobApplicant.Position = new Position();
                jobApplicant.Position.PositionRef = Guid.NewGuid().ToString();

                jobApplicant.Photo = new Photo();
                jobApplicant.Photo.FileName = photoFileName;
                jobApplicant.Photo.PhotoRef = Guid.NewGuid().ToString();
                jobApplicant.Photo.Url = photoUrl;

                jobApplicant.Applicant.BirthDate = postedJobApplication.Applicant.BirthDate;
                jobApplicant.Applicant.EmailAddress = postedJobApplication.Applicant.EmailAddress;
                jobApplicant.Applicant.FullName = postedJobApplication.Applicant.FullName;
                jobApplicant.Position.PositionName = postedJobApplication.Position.PositionName;

                await _jobApplicationRepository.Save(jobApplicant);

                var emotionApiResult = new EmotionApiResult();
                emotionApiResult.JobApplicantRef = jobApplicant.JobApplicationRef;
                emotionApiResult.EmotionApi = emotionResult;

                await _emotionApiRepository.Save(emotionApiResult);

                returnValue.IsSucceed = true;
                returnValue.Result = true;
            }
            catch (Exception exception)
            {
                returnValue.Messages.Add(exception.Message);
            }

            return returnValue;
        }

        public async Task<ServiceResult<bool>> Put(string id, [FromBody]bool isApproved)
        {
            var result = await _jobApplicationRepository.Get(q => q.JobApplicationRef == id);
            var instance = result.FirstOrDefault();
            instance.IsApprovedForHarmony = isApproved;
            await _jobApplicationRepository.Save(instance);

            var returnValue = new ServiceResult<bool>();
            returnValue.IsSucceed = true;
            returnValue.Result = true;
            return returnValue;
        }
    }
}