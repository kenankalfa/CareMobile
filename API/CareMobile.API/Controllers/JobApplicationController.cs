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
using System.Threading.Tasks;

namespace CareMobile.API.Controllers
{
    [MobileAppController]
    public class JobApplicationController : ApiController
    {
        private IJobApplicationRepository _jobApplicationRepository;
        public JobApplicationController(IJobApplicationRepository jobApplicationRepository)
        {
            _jobApplicationRepository = jobApplicationRepository;
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