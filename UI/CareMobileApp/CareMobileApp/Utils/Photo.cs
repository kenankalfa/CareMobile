using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CareMobileApp
{
    public class Photo
    {
        [JsonProperty(PropertyName = "id")]
        public string PhotoRef { get; set; }
        public string FileName { get; set; }
        public string Url { get; set; }
    }
}
