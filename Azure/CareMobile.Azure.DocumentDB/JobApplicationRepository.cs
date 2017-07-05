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
    public class JobApplicationRepository : IJobApplicationRepository
    {
        private IDocumentDBRepository<JobApplication> _repository;

        public JobApplicationRepository(IDocumentDBRepository<JobApplication> repository, IConfigurationSettings settings)
        {
            _repository = repository;
        }

        public Task<IEnumerable<JobApplication>> Get(Expression<Func<JobApplication, bool>> predicate)
        {
            return _repository.GetItemsAsync(predicate);
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
                await _repository.CreateItemAsync(instance);
            }
            else
            {
                await _repository.UpdateItemAsync(instance.JobApplicationRef, instance);
            }
        }
    }
}
