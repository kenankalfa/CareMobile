using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CareMobile.API.Common
{
    public class Photo
    {
        public string PhotoRef { get; set; }
        public string FileName { get; set; }
        public string Url { get; set; }
    }
}
