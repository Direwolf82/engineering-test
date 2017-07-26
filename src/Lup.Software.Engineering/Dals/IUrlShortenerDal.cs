namespace Lup.Software.Engineering.Dals
{
    using System.Threading.Tasks;

    public interface IUrlShortenerDal
    {
        /// <summary>
        /// Verify if the short URL has been used prior.
        /// </summary>
        /// <param name="shortUrl">Short URL being checked for.</param>
        /// <returns>A <see cref="Task"/>. Returns true if short URL is registered, false otherwise.</returns>
        Task<bool> IsShortUrlRegisteredAsync(string shortUrl);

        /// <summary>
        /// Verify if Long URL has been registered with a short URL.
        /// </summary>
        /// <param name="longUrl">Original long url for which query is being made</param>
        /// <returns>A <see cref="Task"/>. Returns True if the long Url is registered, false otherwise. </returns>
        Task<bool> IsLongUrlRegisteredAsync(string longUrl);

        /// <summary>
        /// If short URL exists, will record a further hit for the short URL and return the long URL
        /// </summary>
        /// <param name="shortUrl">Short Url registered against long url</param>
        /// <returns>A <see cref="Task"/>. Returns the long Url string associated with the short Url.</returns>
        Task<string> GetLongUrlAsync(string shortUrl);

        /// <summary>
        /// Returns the shortUrl registered against a long Url.
        /// </summary>
        /// <param name="longUrl">Long URL to find match for</param>
        /// <returns>A <see cref="Task"/> which returns a string representing the long URL.</returns>
        Task<string> GetShortUrlAsync(string longUrl);

        /// <summary>
        /// Given a short url and long url, write to data store and create entry for that long url.
        /// </summary>
        /// <param name="longUrl">Long Url for which to create entry</param>
        /// <param name="shortUrl">Short Url for which to create entry</param>
        /// <returns>A <see cref="Task"/>. Returns true if post successful </returns>
        Task<bool> PostShortUrlToDbAsync(string longUrl, string shortUrl);
    }
}
