namespace RestApiDating.Helpers
{
    public class MensajesParams
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public int UserId { get; set; }
        public string Buzon { get; set; }
    }
}