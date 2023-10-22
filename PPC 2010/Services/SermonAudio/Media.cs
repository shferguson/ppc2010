using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPC_2010.Services.SermonAudio
{
    public class Media
    {
        public class Create
        {
            public string UploadType;
            public string SermonID;

        }

        public class CreateResponse
        {
            public string type;
            public string guid;
            public string sermonID;
            public string uploadType;
            public string destFilename;
            public int priority;
            public int createDate;
            public string uploadURL;
            public string uploadTusBaseURL;
            public bool twoStep;
            public string originalFilename;
            public bool analyzeMetadata;
            public string language;
            public string targetLocation;
            public string s3Endpoint;
        }
    }
}