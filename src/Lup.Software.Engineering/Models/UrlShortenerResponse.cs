namespace Lup.Software.Engineering.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Lup.Software.Engineering.Dals;

    internal class UrlShortenerResponse
    {
        public string ShortUrl { get; set; }

        public string OriginalUrl { get; set; }

        public object Errors { get; set; }
    }
}
