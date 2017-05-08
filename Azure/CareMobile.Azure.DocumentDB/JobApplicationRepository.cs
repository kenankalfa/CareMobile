using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Linq.Expressions;
using CareMobile.API.Common;
using CareMobile.API.Configuration;

namespace CareMobile.Azure.DocumentDB
{
    public class JobApplicationRepository : DocumentDBRepository<JobApplication> , IJobApplicationRepository
    {
        public JobApplicationRepository(IConfigurationSettings settings):base(settings)
        {
            
        }

        public Task<IEnumerable<JobApplication>> Get(Expression<Func<JobApplication, bool>> predicate)
        {
            return GetItemsAsync(predicate);
        }

        public async Task Save(JobApplication instance)
        {
            if (instance == null)
            {
                return;
            }

            if (String.IsNullOrEmpty(instance.JobApplicationRef))
            {
                instance.JobApplicationRef = Guid.NewGuid().ToString();
                await CreateItemAsync(instance);
            }
            else
            {
                await UpdateItemAsync(instance.JobApplicationRef, instance);
            }
        }
    }
}
