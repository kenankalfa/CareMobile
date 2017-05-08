using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CareMobile.Azure.DocumentDB
{
    using CareMobile.API.Common;
    public interface IJobApplicationRepository
    {
        Task<IEnumerable<JobApplication>> Get(Expression<Func<JobApplication,bool>> predicate);
        Task Save(JobApplication instance);
    }
}
