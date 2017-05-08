using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CareMobile.UI.Common
{
    public class Position
    {
        [JsonProperty(PropertyName = "id")]
        public string PositionRef { get; set; }
        public string PositionName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
