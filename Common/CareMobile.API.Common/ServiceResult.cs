﻿using System;
using System.Collections.Generic;

namespace CareMobile.API.Common
{
    public class ServiceResult<T>
    {
        public bool IsSucceed { get; set; }
        public List<string> Messages { get; set; }
        public T Result { get; set; }
    }
}
