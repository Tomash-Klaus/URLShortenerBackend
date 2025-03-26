namespace URLShortenerBackend.DTOs
{
    public class QueryParams
    {
        
        private int maxPageSize = 50;
        private int pageSize = 10;

        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > maxPageSize) ? maxPageSize : value; }
        }

    }
}
