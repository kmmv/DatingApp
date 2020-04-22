namespace DatingApp.API.Helpers
{
     // Class for helping pagination
    public class UserParams
    {
        private const int MaxPageSize = 50;
        // default page number is 1
        public int PageNumber { get; set; } = 1;

        // pagesize is set to 10 client doesn't need to input explicitly
        private int pageSize = 10;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }

        public int UserId { get; set; }
        public string Gender { get; set; }
        public int MinAge { get; set; } = 18;
        public int MaxAge { get; set; } = 99;
        public string OrderBy { get; set; }
        public bool Likees { get; set; } = false;
        public bool Likers { get; set; } = false;
    }
}