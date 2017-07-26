namespace Lup.Software.Engineering.Models
{
    using System.Threading.Tasks;

    public interface IUrlShortener
    {
        Task<string> CreateShortUrl();
    }
}
