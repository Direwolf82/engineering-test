namespace Lup.Software.Engineering.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Lup.Software.Engineering.Dals;

    public class UrlShortener : IUrlShortener
    {
        private const string Chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        private readonly IUrlShortenerDal dal;

        public UrlShortener(IUrlShortenerDal dal)
        {
            this.dal = dal;
        }

        public async Task<string> CreateShortUrl()
        {
            var shortUrl = this.CreateRandomString();
            var urlExists = this.dal.IsShortUrlRegisteredAsync(shortUrl);
            while (await urlExists)
            {
                shortUrl = this.CreateRandomString();
                urlExists = this.dal.IsShortUrlRegisteredAsync(shortUrl);
            }

            return shortUrl;
        }

        private string CreateRandomString()
        {
            var rand = new Random((int)DateTime.Now.Ticks);
            var randString = string.Empty;
            for (var i = 0; i < 7; i++)
            {
                var loc = rand.Next(Chars.Length);
                randString += Chars[loc];
            }

            return randString;
        }
    }
}
