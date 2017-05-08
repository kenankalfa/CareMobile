using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CareMobile.API.Common
{
    public class JobApplication
    {
        [JsonProperty(PropertyName = "id")]
        public string JobApplicationRef { get; set; }
        public Position Position { get; set; }
        public Applicant Applicant { get; set; }
        public Photo Photo { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool? IsApprovedForHarmony { get; set; }
    }
}
