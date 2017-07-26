namespace Lup.Software.Engineering.Dals
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Lup.Software.Engineering.AzureAccess;
    using Microsoft.Extensions.Configuration;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;

    public class UrlShortenerDal : IUrlShortenerDal
    {
        private readonly CloudTableClient tableClient;
        private readonly CloudTable table;

        public UrlShortenerDal(IConfiguration config)
        {
            var connString = config.GetConnectionString("TableStorage");
            var cloudStorageAccount = CloudStorageAccount.Parse(connString);
            this.tableClient = cloudStorageAccount.CreateCloudTableClient();

            this.table = this.tableClient.GetTableReference("ShortUrl");
            this.table.CreateIfNotExistsAsync();
        }

        public async Task<string> GetLongUrlAsync(string shortUrl)
        {
            try
            {
                var operation = TableOperation.Retrieve(shortUrl, string.Empty);
                var result = await this.table.ExecuteAsync(operation);
                if (result.Result != null)
                {
                    var entity = (ShortUrlEntity)result.Result;
                    entity.Hits += 1;
                    operation = TableOperation.Replace(entity);
                    result = await this.table.ExecuteAsync(operation);
                    return entity.LongUrl;
                }
                else
                {
                    throw new StorageException(string.Format("Short URL {0} not registered", shortUrl));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> GetShortUrlAsync(string longUrl)
        {
            try
            {
                var operation = TableOperation.Retrieve(string.Empty, longUrl);
                var result = await this.table.ExecuteAsync(operation);
                if (result.Result != null)
                {
                    var entity = (ShortUrlEntity)result.Result;
                    return entity.ShortUrl;
                }
                else
                {
                    throw new StorageException(string.Format("Long URL {0} not registered", longUrl));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> IsLongUrlRegisteredAsync(string longUrl)
        {
            var operation = TableOperation.Retrieve(string.Empty, longUrl);
            var result = await this.table.ExecuteAsync(operation);
            if (result.Result != null)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> IsShortUrlRegisteredAsync(string shortUrl)
        {
            try
            {
                var operation = TableOperation.Retrieve(shortUrl, string.Empty);
                var result = await this.table.ExecuteAsync(operation);
                if (result.Result != null)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> PostShortUrlToDbAsync(string longUrl, string shortUrl)
        {
            var entity = new ShortUrlEntity(shortUrl, longUrl)
            {
                ShortUrl = shortUrl,
                LongUrl = longUrl,
                Hits = 0
            };
            var operation = TableOperation.Insert(entity);
            var result = await this.table.ExecuteAsync(operation);
            if (result.HttpStatusCode == 200)
            {
                return true;
            }

            return false;
        }
    }
}
