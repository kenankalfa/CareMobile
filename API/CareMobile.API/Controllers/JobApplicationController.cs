using Microsoft.Azure.Mobile.Server.Config;
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

namespace CareMobile.API.Controllers
{
    [MobileAppController]
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

                var multiPartContent = Request.Content as MultipartFormDataContent;

                if (multiPartContent == null)
                {
                    throw new Exception("Request.Content is not MultipartFormDataContent");
                }

                if (multiPartContent.Any())
                {
                    throw new Exception("Posted data is empty");
                }

                var streamContent = multiPartContent.ElementAt(0) as StreamContent;
                var formDataContent = multiPartContent.ElementAt(1) as StringContent;

                var photoStream = await streamContent.ReadAsStreamAsync();
                var entityAsString = await formDataContent.ReadAsStringAsync();

                var postedJobApplication = JsonConvert.DeserializeObject<JobApplication>(entityAsString);

                var photoFileName = Guid.NewGuid().ToString() + ".jpg";
                var photoUrl = _storageRepository.UploadFile(photoFileName,photoStream);

                var emotionResult = await _emotionRepository.GetEmotion(photoUrl);

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
            



            // todo :
            // get httpmultipartrequest
            // split content
            // content : stream photo
            // content : instance job application
            // azure connect
            // azure document db
            // azure storage
            return null;
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