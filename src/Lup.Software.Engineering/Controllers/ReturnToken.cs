﻿namespace Lup.Software.Engineering.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Lup.Software.Engineering.Dals;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using Lup.Software.Engineering.Models;

    public class ReturnToken
    {
        public string OriginalUrl { get; set; }

        public string ShortUrl { get; set; }

        public Dictionary<string, string> Errors { get; set; }
    }
}
