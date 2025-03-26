using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace URLShortenerBackend.Models
{
    public class UrlData : BasicEntity
    {
        public string FullUrl { get; set; }
        public string ShortUrl {  get; set; }
        [ForeignKey("User")]
        public string CreatedBy { get; set; }
        public DateOnly CreatedDate { get; set; }

        public IdentityUser User { get; set; }

    }
}
