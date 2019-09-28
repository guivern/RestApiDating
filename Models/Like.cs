namespace RestApiDating.Models
{
    public class Like
    {
        public int LikerId { get; set; } // usuario que da like
        public int LikedId { get; set; } // usuario que recibe like
        public User Liker { get; set; }
        public User Liked { get; set; }
    }
}