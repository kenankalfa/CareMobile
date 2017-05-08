using Newtonsoft.Json;
using System;

namespace CareMobile.API.Common
{
    public class Applicant
    {
        [JsonProperty(PropertyName ="id")]
        public string ApplicantRef { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string EmailAddress { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
