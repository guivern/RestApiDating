namespace RestApiDating.Models
{
    public class Like
    {
        public int LikerId { get; set; } // usuario que da like
        public int LikedId { get; set; } // usuario que recibe like
        public virtual User Liker { get; set; }
        public virtual User Liked { get; set; }
    }
}