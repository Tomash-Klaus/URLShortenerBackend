using System.Net;

namespace URLShortenerBackend.DTOs
{
    public class ErrorResponse
    {
        public int Code { get; set; }
        public string Message { get; set; } = "Oops! Something gone wrong!";
    }
}
