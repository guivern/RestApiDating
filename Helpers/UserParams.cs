namespace RestApiDating.Helpers
{
    public class UserParams
    {
        private const int MAX_PAGE_SIZE = 50;
        private const int DEFAULT_PAGE_SIZE = 5;
        private const int DEFAULT_PAGE_NUMBER = 1;

        public int PageNumber { get; set; } = DEFAULT_PAGE_NUMBER;
        private int pageSize = DEFAULT_PAGE_SIZE;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MAX_PAGE_SIZE) ? MAX_PAGE_SIZE : value; }
        }
        public int UserId { get; set; }
        public string Genero { get; set; }
        public int EdadMin { get; set; } = 18;
        public int EdadMax { get; set; } = 99;
        public string OrderBy { get; set; }
    }
}