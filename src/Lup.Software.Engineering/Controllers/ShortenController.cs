namespace Lup.Software.Engineering.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Lup.Software.Engineering.Dals;
    using Lup.Software.Engineering.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    [Controller]
    public class ShortenController : Controller
    {
        private readonly IUrlShortenerDal dal;
        private readonly IUrlShortener urlShortener;

        public ShortenController(IUrlShortenerDal dal, IUrlShortener shortener)
        {
            this.dal = dal;
            this.urlShortener = shortener;
        }

        [HttpPost]
        [Route("api/shorten")]
        public async Task<IActionResult> Shorten(string token)
        {
            string originalUrl;
            ReturnToken response = new ReturnToken();
            Task<bool> longUrlExists;
            try
            {
                originalUrl = JsonConvert.DeserializeObject<PostToken>(token).OriginalUrl;
            }
            catch
            {
                response.ShortUrl = null;
                response.OriginalUrl = null;
                response.Errors = new Dictionary<string, string>
                    {
                        { "Token", "The token could not be parsed as expected." }
                    };
                return this.BadRequest(JsonConvert.SerializeObject(response));
            }

            response.OriginalUrl = originalUrl;
            if (!this.IsUrlValid(originalUrl))
            {
                response.ShortUrl = null;
                response.Errors = new Dictionary<string, string>
                    {
                        { "OriginalUrl", "The OriginalUrl field is not a valid fully-qualified http, https, or ftp URL." }
                    };
                return this.BadRequest(JsonConvert.SerializeObject(response));
            }

            longUrlExists = this.dal.IsLongUrlRegisteredAsync(originalUrl);
            if (await longUrlExists)
            {
                var shortUrl = this.dal.GetShortUrlAsync(originalUrl);
                response.Errors = null;
                response.ShortUrl = await shortUrl;
                return this.Ok(JsonConvert.SerializeObject(response));
            }
            else
            {
                var shortUrl = this.urlShortener.CreateShortUrl();
                var postedUrl = this.dal.PostShortUrlToDbAsync(originalUrl, await shortUrl);
                response.Errors = null;
                response.ShortUrl = await shortUrl;
                return this.Ok(JsonConvert.SerializeObject(response));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetShortUrl(string shortUrl)
        {
            try
            {
                var longUrl = await this.dal.GetLongUrlAsync(shortUrl);
                if (string.IsNullOrEmpty(longUrl))
                {
                    return this.Redirect(longUrl);
                }
                else
                {
                    var response = new NotFoundResult();
                    return this.NotFound();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool IsUrlValid(string url)
        {
            return Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute);
        }
    }
}
