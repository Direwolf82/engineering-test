namespace Lup.Software.Engineering.AzureAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.WindowsAzure.Storage.Table;

    public class ShortUrlEntity : TableEntity
    {
        public ShortUrlEntity(string shortUrl, string longUrl)
        {
            this.PartitionKey = shortUrl;
            this.RowKey = longUrl;
        }

        public ShortUrlEntity()
        {
        }

        public string LongUrl { get; set; }

        public string ShortUrl { get; set; }

        public int Hits { get; set; }
    }
}
