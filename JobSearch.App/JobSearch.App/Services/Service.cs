﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace JobSearch.App.Services
{
    public class Service
    {
        protected HttpClient _client;
        protected string BaseApiUrl = "https://jobsearchapi20231218130706.azurewebsites.net";

        public Service()
        {
            _client = new HttpClient();
        }
    }
}
