namespace TypeRunnerBE.Models
{
    public class UserFriend
    {
        public int FromUserId { get; set; }
        public User FromUser { get; set; }

        public int ToUserId { get; set; }
        public User ToUser { get; set; }
    }
}
