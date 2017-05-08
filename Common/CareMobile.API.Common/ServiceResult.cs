using System;

namespace CareMobile.API.Common
{
    public class ServiceResult<T>
    {
        public bool IsSucceed { get; set; }
        public string Messages { get; set; }
        public T Result { get; set; }
    }
}
