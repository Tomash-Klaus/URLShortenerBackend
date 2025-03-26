namespace URLShortenerBackend.Services
{
    public interface IUrlShortenerService
    {
        public string GenerateShortUrl(string longUrl);
    }
}
