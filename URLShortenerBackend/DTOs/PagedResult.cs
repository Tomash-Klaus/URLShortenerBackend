namespace URLShortenerBackend.DTOs
{
    public class PagedResult<T> where T : class
    {
        public int PageSize {  get; set; }
        public int PageNumber { get; set; }
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }
    }
}
