﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareMobile.API.Common
{
    public class FaceRectangle
    {
        public int height { get; set; }
        public int left { get; set; }
        public int top { get; set; }
        public int width { get; set; }
    }

    public class Scores
    {
        public double anger { get; set; }
        public double contempt { get; set; }
        public double disgust { get; set; }
        public double fear { get; set; }
        public double happiness { get; set; }
        public double neutral { get; set; }
        public double sadness { get; set; }
        public double surprise { get; set; }
    }

    public class RootObject
    {
        public FaceRectangle faceRectangle { get; set; }
        public Scores scores { get; set; }
    }

    public class EmotionApiResult
    {
        public RootObject EmotionApi { get; set; }
        public string JobApplicantRef { get; set; }
    }

}
